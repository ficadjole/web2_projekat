import type { User } from "../models/user/User";

export interface EditUserModalProps {
  user: User | null;
  onClose: () => void;
  onSave: (id: string, name: string, email: string) => Promise<void>;
}
