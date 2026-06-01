import type { UserRowProps } from "../../props/UserRowProps";
import { RoleDropdown } from "./RoleDropdown";

export function UserRow({
  user,
  onEdit,
  onDelete,
  onRoleChange,
}: UserRowProps) {
  return (
    <tr className="hover:bg-slate-50 transition-colors duration-100">
      <td className="px-6 py-4">
        <div className="flex items-center gap-3">
          <div
            className="w-9 h-9 rounded-full bg-gradient-to-br from-blue-400 to-cyan-400
            flex items-center justify-center text-white font-semibold text-sm flex-shrink-0"
          >
            {user.name.charAt(0).toUpperCase()}
          </div>
          <span className="font-medium text-slate-800">{user.name}</span>
        </div>
      </td>

      <td className="px-6 py-4 text-slate-500 text-sm">{user.email}</td>

      <td className="px-6 py-4">
        <RoleDropdown
          userId={user.id}
          currentRole={user.role}
          onRoleChange={onRoleChange}
        />
      </td>

      <td className="px-6 py-4">
        <div className="flex items-center gap-2 justify-end">
          <button
            onClick={() => onEdit(user)}
            className="p-2 rounded-lg text-slate-400 hover:text-blue-500
              hover:bg-blue-50 transition-all duration-150"
            title="Edit user"
          >
            <svg
              className="w-4 h-4"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z"
              />
            </svg>
          </button>

          <button
            onClick={() => onDelete(user.id)}
            className="p-2 rounded-lg text-slate-400 hover:text-red-500
              hover:bg-red-50 transition-all duration-150"
            title="Delete user"
          >
            <svg
              className="w-4 h-4"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"
              />
            </svg>
          </button>
        </div>
      </td>
    </tr>
  );
}
