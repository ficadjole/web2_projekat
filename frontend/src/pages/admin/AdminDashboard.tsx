import { useState } from "react";
import { useAdminUsers } from "../../hooks/user/useAdminUsers";
import type { User } from "../../models/user/User";
import { UserTable } from "../../components/admin/UserTable";
import { EditUserModal } from "../../components/admin/EditUserModal";
import { Navbar } from "../../components/layout/Navbar";

export function AdminDashboardPage() {
  const { users, isLoading, error, updateUser, updateRole, deleteUser } =
    useAdminUsers();
  const [selectedUser, setSelectedUser] = useState<User | null>(null);

  return (
    <div className="min-h-screen bg-slate-50">
      <Navbar />

      <main className="max-w-6xl mx-auto px-6 py-8">
        <div className="mb-6">
          <h1 className="text-2xl font-bold text-slate-800">Users</h1>
          <p className="text-slate-500 text-sm mt-1">
            Manage user accounts and permissions
          </p>
        </div>

        <div className="grid grid-cols-2 gap-4 mb-6 max-w-sm">
          <div className="bg-white rounded-xl border border-slate-100 shadow-sm px-5 py-4">
            <p className="text-xs text-slate-500 uppercase tracking-wide font-medium">
              Total Users
            </p>
            <p className="text-2xl font-bold text-slate-800 mt-1">
              {users.length}
            </p>
          </div>
          <div className="bg-white rounded-xl border border-slate-100 shadow-sm px-5 py-4">
            <p className="text-xs text-slate-500 uppercase tracking-wide font-medium">
              Admins
            </p>
            <p className="text-2xl font-bold text-slate-800 mt-1">
              {users.filter((u) => u.role === "Admin").length}
            </p>
          </div>
        </div>

        {error && (
          <div className="mb-4 px-4 py-3 bg-red-50 border border-red-100 rounded-xl text-red-600 text-sm">
            {error}
          </div>
        )}

        {isLoading ? (
          <div className="flex items-center justify-center py-20">
            <div className="flex flex-col items-center gap-3 text-slate-400">
              <svg
                className="animate-spin w-6 h-6"
                viewBox="0 0 24 24"
                fill="none"
              >
                <circle
                  className="opacity-25"
                  cx="12"
                  cy="12"
                  r="10"
                  stroke="currentColor"
                  strokeWidth="4"
                />
                <path
                  className="opacity-75"
                  fill="currentColor"
                  d="M4 12a8 8 0 018-8v8z"
                />
              </svg>
              <span className="text-sm">Loading users...</span>
            </div>
          </div>
        ) : (
          <UserTable
            users={users}
            onEdit={setSelectedUser}
            onDelete={deleteUser}
            onRoleChange={updateRole}
          />
        )}
      </main>

      <EditUserModal
        user={selectedUser}
        onClose={() => setSelectedUser(null)}
        onSave={updateUser}
      />
    </div>
  );
}
