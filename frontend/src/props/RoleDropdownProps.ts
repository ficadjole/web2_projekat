export interface RoleDropdownProps {
  userId: string;
  currentRole: "User" | "Admin";
  onRoleChange: (id: string, role: "User" | "Admin") => void;
}
