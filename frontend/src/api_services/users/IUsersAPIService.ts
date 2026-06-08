import type { UpdateRoleRequest } from "../../dtos/UpdateRoleRequest";
import type { UpdateUserRequest } from "../../dtos/UpdateUserRequest";
import type { User } from "../../models/user/User";

export interface IUsersAPIService {
  getAllUsers(): Promise<User[]>;
  updateUser(id: string, data: UpdateUserRequest): Promise<User>;
  updateRole(id: string, data: UpdateRoleRequest): Promise<User>;
  deleteUser(id: string): Promise<void>;
  getUserById(id: string): Promise<User>;
}
