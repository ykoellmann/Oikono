import './App.css'
import {RouterProvider} from "react-router-dom";
import {AuthProvider} from "@/auth/auth-context.tsx";
import {router} from "@/router";
import { ThemeProvider } from './components/theme-provider';
import {Toaster} from "@/components/ui/sonner.tsx";

export default function App() {
    return (
        <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
            <AuthProvider>
                <RouterProvider router={router}/>
            </AuthProvider>
            <Toaster
                richColors
                position="bottom-right"
                duration={3000}
            />
        </ThemeProvider>
    )
}
