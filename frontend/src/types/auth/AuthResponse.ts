import type { User } from "../../models/user/User";

export interface AuthResponse {
  token: string;
  user: User;
}
