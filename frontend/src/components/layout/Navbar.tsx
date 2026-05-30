import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../../hooks/auth/useAuthHook";

export function Navbar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  return (
    <nav className="bg-white border-b border-slate-100 shadow-sm sticky top-0 z-40">
      <div className="max-w-6xl mx-auto px-6 py-3 flex items-center justify-between">
        <Link to="/home" className="flex items-center gap-2 group">
          <div
            className="w-8 h-8 rounded-xl bg-gradient-to-br from-blue-500 to-cyan-400
            flex items-center justify-center shadow-sm group-hover:scale-105 transition-transform"
          >
            <span className="text-white text-sm">🌍</span>
          </div>
          <span className="font-bold text-slate-800 tracking-tight">
            TripPlanner
          </span>
        </Link>

        <div className="flex items-center gap-1">
          <Link
            to="/profile"
            className="px-4 py-2 rounded-lg text-sm font-medium text-slate-600
              hover:bg-slate-50 hover:text-slate-800 transition-colors"
          >
            Profile
          </Link>

          {user?.role === "Admin" && (
            <Link
              to="/admin"
              className="px-4 py-2 rounded-lg text-sm font-medium text-purple-600
                hover:bg-purple-50 transition-colors"
            >
              Admin Dashboard
            </Link>
          )}
        </div>

        <div className="flex items-center gap-3">
          <div className="flex items-center gap-2">
            <div
              className="w-8 h-8 rounded-full bg-gradient-to-br from-blue-400 to-cyan-400
              flex items-center justify-center text-white text-sm font-semibold"
            >
              {user?.name?.charAt(0).toUpperCase()}
            </div>
            <span className="text-sm font-medium text-slate-700 hidden sm:block">
              {user?.name}
            </span>
          </div>
          <button
            onClick={handleLogout}
            className="px-3 py-2 rounded-lg text-sm text-slate-500
              hover:bg-sky-100 hover:text-slate-700 transition-colors border-dashed border-1"
          >
            Sign out
          </button>
        </div>
      </div>
    </nav>
  );
}
