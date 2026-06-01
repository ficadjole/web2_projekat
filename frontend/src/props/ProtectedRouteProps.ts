export interface ProtectedRouteProps {
  children: React.ReactNode;
  requiredRole?: "User" | "Admin";
}
