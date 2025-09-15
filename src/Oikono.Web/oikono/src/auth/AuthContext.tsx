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
    accessToken: string;
    refreshToken?: { token: string; expires: string };
    user?: User;
};

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
                if (data?.accessToken) {
                    localStorage.setItem("access_token", data.accessToken);
                    api.setAuthToken(data.accessToken);
                    if (data?.refreshToken?.token) {
                        localStorage.setItem("refresh_token", data.refreshToken.token);
                    }
                    if (data?.user) setUser(data.user);
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

        localStorage.setItem("access_token", data.accessToken);
        api.setAuthToken(data.accessToken);
        if (data?.refreshToken?.token) {
            localStorage.setItem("refresh_token", data.refreshToken.token);
        }

        if (data?.user) setUser(data.user);

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
