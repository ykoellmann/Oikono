import { Navigate, Outlet, useLocation } from "react-router-dom";
import { useAuth } from "@/auth/auth-context.tsx";
import { Spinner } from "@/components/ui/shadcn-io/spinner";

export default function ProtectedRoute() {
  const { isAuthenticated, loading } = useAuth();
  const location = useLocation();

    if (loading) {
        return (
            <div className="flex items-center justify-center h-screen w-full">
              <Spinner variant="ring" className="size-15" />
            </div>
        )
    }

  if (!isAuthenticated) {
    return <Navigate to="/login" replace state={{ from: location }} />;
  }

  return <Outlet />;
}
