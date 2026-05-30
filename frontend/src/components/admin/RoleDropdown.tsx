interface RoleDropdownProps {
  userId: string;
  currentRole: "User" | "Admin";
  onRoleChange: (id: string, role: "User" | "Admin") => void;
}

export function RoleDropdown({
  userId,
  currentRole,
  onRoleChange,
}: RoleDropdownProps) {
  return (
    <select
      value={currentRole}
      onChange={(e) => onRoleChange(userId, e.target.value as "User" | "Admin")}
      className={`
        px-3 py-1.5 rounded-lg text-sm font-medium border cursor-pointer
        focus:outline-none focus:ring-2 focus:ring-blue-300
        transition-colors duration-150
        ${
          currentRole === "Admin"
            ? "bg-purple-50 text-purple-700 border-purple-200 hover:bg-purple-100"
            : "bg-blue-50 text-blue-700 border-blue-200 hover:bg-blue-100"
        }
      `}
    >
      <option value="User">User</option>
      <option value="Admin">Admin</option>
    </select>
  );
}
