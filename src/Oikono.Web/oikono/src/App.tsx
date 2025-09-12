import './App.css'
import {createBrowserRouter, Navigate, RouterProvider} from "react-router-dom";
import HomePage from "@/pages/home.tsx";
import RecipesPage from "@/pages/recipes";
import NotFoundPage from "@/pages/not-found.tsx";
import RecipeDetailPage from "@/pages/recipes/detail.tsx";

// eslint-disable-next-line react-refresh/only-export-components
export const router = createBrowserRouter([
    { index: true, element: <HomePage /> },
    { path: "recipes", element: <RecipesPage /> },
    { path: "recipes/:id", element: <RecipeDetailPage /> },
    { path: "/home", element: <Navigate to="/" replace /> },
    { path: "*", element: <NotFoundPage /> },
]);

export default function App() {
    return <RouterProvider router={router} />
}
