import type { User } from "../models/user/User";

export interface UserRowProps {
  user: User;
  onEdit: (user: User) => void;
  onDelete: (id: string) => void;
  onRoleChange: (id: string, role: "User" | "Admin") => void;
}
