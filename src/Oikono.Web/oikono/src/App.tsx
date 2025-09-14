import './App.css'
import {RouterProvider} from "react-router-dom";
import {AuthProvider} from "@/auth/AuthContext";
import {router} from "@/router";
import { ThemeProvider } from './components/theme-provider';

export default function App() {
    return (
        <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
            <AuthProvider>
                <RouterProvider router={router}/>
            </AuthProvider>
        </ThemeProvider>
    )
}
