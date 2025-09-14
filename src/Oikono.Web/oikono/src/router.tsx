import { createBrowserRouter, Navigate } from "react-router-dom";
import HomePage from "@/pages/home";
import RecipesPage from "@/pages/recipes";
import NotFoundPage from "@/pages/not-found";
import RecipeDetailPage from "@/pages/recipes/detail";
import LoginPage from "@/pages/auth/login";
import RegisterPage from "@/pages/auth/register";
import ProtectedRoute from "@/auth/ProtectedRoute";

export const router = createBrowserRouter([
  { path: "/login", element: <LoginPage /> },
  { path: "/register", element: <RegisterPage /> },
  {
    element: <ProtectedRoute />,
    children: [
      { index: true, element: <HomePage /> },
      { path: "recipes", element: <RecipesPage /> },
      { path: "recipes/:id", element: <RecipeDetailPage /> },
      { path: "home", element: <Navigate to="/" replace /> },
    ],
  },
  { path: "*", element: <NotFoundPage /> },
]);
