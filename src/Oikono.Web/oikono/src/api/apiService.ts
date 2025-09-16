import axios, {
    AxiosError,
    AxiosHeaders,
    type AxiosInstance,
    type AxiosRequestConfig,
    type AxiosResponse,
    type InternalAxiosRequestConfig,
} from "axios";
import {v4 as uuidv4} from "uuid";
import {toast} from "sonner";

const API_BASE_URL =
    (import.meta.env.VITE_API_BASE_URL as string) || "/api";

// Cookies needed for httpOnly refresh cookie

type Method = "get" | "post" | "put" | "patch" | "delete" | "options" | "head";

type RetryableConfig = InternalAxiosRequestConfig & {
    _retry?: boolean;
};

function isWriteMethod(method?: string | null): method is Method {
    const m = (method || "").toLowerCase();
    return m === "post" || m === "put" || m === "patch" || m === "delete";
}

function setHeader(headers: AxiosHeaders | Record<string, unknown> | undefined, key: string, value: string) {
    if (!headers) return;
    if (headers instanceof AxiosHeaders || typeof (headers as unknown as {
        set?: (k: string, v: string) => void
    }).set === "function") {
        (headers as AxiosHeaders).set(key, value);
    } else {
        (headers as Record<string, unknown>)[key] = value;
    }
}

class ApiService {
    public setAuthToken(token?: string) {
        if (token) {
            this.client.defaults.headers.common["Authorization"] = `Bearer ${token}`;
        } else {
            delete this.client.defaults.headers.common["Authorization"];
        }
    }

    private client: AxiosInstance;
    private refreshClient: AxiosInstance;

    // Verhindert parallele Refresh-Calls und queued Requests bis Refresh fertig ist
    private isRefreshing = false;
    private refreshQueue: Array<(token: string) => void> = [];
    private refreshRejectQueue: Array<(err: unknown) => void> = [];

    constructor() {
        this.client = axios.create({
            baseURL: API_BASE_URL,
            withCredentials: true,
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json",
            },
            timeout: 30000,
        });

        // Separater Client ohne Interceptors für den Token-Refresh
        this.refreshClient = axios.create({
            baseURL: API_BASE_URL,
            withCredentials: true,
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json",
            },
            timeout: 15000,
        });

        this.client.interceptors.request.use(
            this.handleRequest.bind(this),
            (error) => Promise.reject(error),
        );

        this.client.interceptors.response.use(
            (res) => res,
            this.handleResponseError.bind(this),
        );
    }

    private handleRequest(config: InternalAxiosRequestConfig): InternalAxiosRequestConfig {
        // Always attach a request id for backend validation
        setHeader(config.headers, "X-Request-Id", uuidv4());

        // JWT anhängen
        const token = localStorage.getItem("access_token");
        if (token) {
            setHeader(config.headers, "Authorization", `Bearer ${token}`);
        }

        // Idempotency-Key für schreibende Methoden
        if (isWriteMethod(config.method)) {
            setHeader(config.headers, "Idempotency-Key", uuidv4());
        }

        return config;
    }

    private async handleResponseError(error: AxiosError): Promise<never | AxiosResponse> {
        const response = error.response;
        const originalConfig = error.config as RetryableConfig | undefined;

        if (!response || !originalConfig) {
            toast.error("Network error", {
                description: "Please check your internet connection and try again.",
            });
            return Promise.reject(error);
        }

        // 401 abfangen und Access-Token per Refresh-Token erneuern
        if (response.status === 401 && !originalConfig._retry) {
            // Markiere Original-Request als "wird wiederholt"
            originalConfig._retry = true;

            try {
                const newAccessToken = await this.refreshAccessToken();

                // Authorization für den Original-Request setzen und erneut senden
                setHeader(originalConfig.headers, "Authorization", `Bearer ${newAccessToken}`);
                return this.client(originalConfig);
            } catch (refreshErr) {
                this.forceLogout();
                return Promise.reject(refreshErr);
            }
            //am besten nur rerouten wenn man nicht bei login/re
        }

        // Für alle anderen Fehler (nicht 401) differenziertes Handling mit ProblemDetails/ValidationErrors
        try {
            type ProblemDetails = {
                type?: string;
                title?: string;
                status?: number;
                detail?: string;
                instance?: string;
                errors?: Record<string, string[]>; // ASP.NET Core validation dictionary
            };
            const data = response.data as unknown as ProblemDetails | undefined;

            // Spezialfall: Konto nicht aktiviert -> freundliche, klare Meldung
            if (data?.errors && data.errors["Authentication.AccountNotActive"]) {
                const msg = data.errors["Authentication.AccountNotActive"][0] || "Ihr Konto ist noch nicht aktiviert.";
                toast.error("Konto nicht aktiviert", { description: msg });
            }
            // Allgemeine Validierungsfehler (400) mit Fehlerliste anzeigen
            else if (response.status === 400 && data?.errors && typeof data.errors === "object") {
                // Flache Liste der ersten Fehlermeldungen je Feld
                const messages: string[] = [];
                for (const key of Object.keys(data.errors)) {
                    const first = data.errors[key]?.[0];
                    if (first) messages.push(first);
                }
                const description = messages.length ? messages.join("\n") : (data.detail || data.title || "Ungültige Eingabe.");
                toast.error("Eingabe prüfen", { description });
            }
            // Sonst generische ProblemDetails verwenden
            else {
                const description = data?.detail || data?.title || "Es ist ein Fehler aufgetreten.";
                toast.error("Uh oh! Something went wrong.", { description });
            }
        } catch {
            // Fallback, falls response.data nicht lesbar ist
            toast.error("Uh oh! Something went wrong.");
        }

        return Promise.reject(error);
    }

    private async refreshAccessToken(): Promise<string> {
        if (this.isRefreshing) {
            // Warte, bis der laufende Refresh fertig ist
            return new Promise<string>((resolve, reject) => {
                this.refreshQueue.push(resolve);
                this.refreshRejectQueue.push(reject);
            });
        }

        this.isRefreshing = true;

        try {
            // Backend: reads httpOnly refresh cookie; no body needed
            const {data} = await this.refreshClient.post<{ token: string }>(
                "/authentication/token/refresh",
                undefined,
                {headers: {"X-Request-Id": uuidv4()}}
            );

            const newAccessToken = data.token;
            localStorage.setItem("access_token", newAccessToken);
            // update default header immediately
            this.client.defaults.headers.common["Authorization"] = `Bearer ${newAccessToken}`;

            // Alle wartenden Requests fortsetzen
            this.refreshQueue.forEach((cb) => cb(newAccessToken));
            this.refreshQueue = [];
            this.refreshRejectQueue = [];

            return newAccessToken;
        } catch (err) {
            // Prüfe ProblemDetails-Titel und erzwinge Logout bei ungültigem/abgelaufenem Refresh Token
            const maybeAxios = err as AxiosError<{ title?: string }>;
            const title = maybeAxios?.response?.data?.title;
            if (title === "Authentication.InvalidRefreshToken" || title === "Authentication.RefreshTokenExpired") {
                this.forceLogout();
            }
            // Alle wartenden Requests ablehnen
            this.refreshRejectQueue.forEach((rej) => rej(err));
            this.refreshQueue = [];
            this.refreshRejectQueue = [];
            throw err;
        } finally {
            this.isRefreshing = false;
        }
    }

    private forceLogout() {
        localStorage.removeItem("access_token");
        localStorage.removeItem("refresh_token");
        // Optional: zu Login leiten
        if (window.location.pathname !== "/login" && window.location.pathname !== "/register") window.location.href = "/login";
    }

    // Public API – gibt standardmäßig .data zurück
    public async get<T>(url: string, cfg?: AxiosRequestConfig): Promise<T> {
        const res = await this.client.get<T>(url, cfg);
        return res.data;
    }

    public async post<T>(url: string, body?: unknown, cfg?: AxiosRequestConfig): Promise<T> {
        const res = await this.client.post<T>(url, body, cfg);
        return res.data;
    }

    public async put<T>(url: string, body?: unknown, cfg?: AxiosRequestConfig): Promise<T> {
        const res = await this.client.put<T>(url, body, cfg);
        return res.data;
    }

    public async patch<T>(url: string, body?: unknown, cfg?: AxiosRequestConfig): Promise<T> {
        const res = await this.client.patch<T>(url, body, cfg);
        return res.data;
    }

    public async delete<T>(url: string, cfg?: AxiosRequestConfig): Promise<T> {
        const res = await this.client.delete<T>(url, cfg);
        return res.data as unknown as T;
    }
}

const api = new ApiService();
export default api;
