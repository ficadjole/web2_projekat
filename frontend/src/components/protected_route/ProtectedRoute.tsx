import { Navigate } from "react-router-dom";
import { useAuth } from "../../hooks/auth/useAuthHook";
import type { ProtectedRouteProps } from "../../props/ProtectedRouteProps";

const ProtectedRoute = ({ children, requiredRole }: ProtectedRouteProps) => {
  const { isAuthenticated, isLoading, user } = useAuth();

  if (isLoading) return <div className="loading">Loading...</div>;

  if (!isAuthenticated) return <Navigate to="/login" replace />;

  if (requiredRole && user?.role !== requiredRole)
    return <Navigate to="/dashboard" replace />;

  return <>{children}</>;
};

export default ProtectedRoute;
