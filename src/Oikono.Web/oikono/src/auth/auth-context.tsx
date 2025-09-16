/* eslint-disable react-refresh/only-export-components */
import React, {createContext, useCallback, useContext, useEffect, useMemo, useState} from "react";
import api from "@/api/apiService";
import {v4 as uuidv4} from "uuid";

export type User = {
    id: string;
    email: string;
    firstName?: string;
    lastName?: string;
};

export type AuthResponse = {
    token: string;
};

function getCookie(name: string): string | undefined {
    const cookies = document.cookie ? document.cookie.split(";") : [];
    const prefix = name + "=";
    for (let c of cookies) {
        c = c.trim();
        if (c.startsWith(prefix)) {
            return decodeURIComponent(c.substring(prefix.length));
        }
    }
    return undefined;
}

type AuthContextValue = {
    user: User | null;
    isAuthenticated: boolean;
    loading: boolean;
    login: (email: string, password: string, redirectTo?: string) => Promise<void>;
    register: (data: { firstName: string; lastName: string; email: string; password: string }, redirectTo?: string) => Promise<void>;
    logout: () => void;
};

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export function AuthProvider({children}: { children: React.ReactNode }) {
    const [user, setUser] = useState<User | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        // try to refresh on load; backend uses httpOnly cookie for refresh
        const bootstrap = async () => {
            try {
                const data = await api.post<AuthResponse>("/authentication/token/refresh");
                if (data?.token) {
                    localStorage.setItem("access_token", data.token);
                    api.setAuthToken(data.token);
                    // refreshToken kommt jetzt als Cookie namens "refreshToken"
                    const rt = getCookie("refreshToken");
                    if (rt) {
                        localStorage.setItem("refresh_token", rt);
                    }
                }
            } catch {
                // ignore
            } finally {
                setLoading(false);
            }
        };
        bootstrap();
    }, []);

    const authenticate = useCallback(async (endpoint: string, payload: object, redirectTo?: string) => {
        const data = await api.post<AuthResponse>(endpoint, payload, {headers: {"X-Request-Id": uuidv4()}});

        localStorage.setItem("access_token", data.token);
        api.setAuthToken(data.token);
        // refreshToken kommt jetzt als Cookie namens "refreshToken"
        const rt = getCookie("refreshToken");
        if (rt) {
            localStorage.setItem("refresh_token", rt);
        }

        // Weiterleitung nach Login/Register
        window.location.href = redirectTo || "/";
    }, []);

    const login = useCallback(
        (email: string, password: string, redirectTo?: string) => authenticate("/authentication/login", {
            email,
            password
        }, redirectTo),
        [authenticate]
    );

    const register = useCallback(
        (payload: { firstName: string; lastName: string; email: string; password: string }, redirectTo?: string) =>
            authenticate("/authentication/register", payload, redirectTo),
        [authenticate]
    );

    const logout = useCallback(() => {
        localStorage.removeItem("access_token");
        localStorage.removeItem("refresh_token");
        api.setAuthToken(undefined);
        setUser(null);
        window.location.href = "/login";
    }, []);

    const value = useMemo(() => ({
        user,
        isAuthenticated: Boolean(localStorage.getItem("access_token")),
        loading,
        login,
        register,
        logout,
    }), [user, loading, login, register, logout]);

    return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
    const ctx = useContext(AuthContext);
    if (!ctx) throw new Error("useAuth must be used within AuthProvider");
    return ctx;
}
