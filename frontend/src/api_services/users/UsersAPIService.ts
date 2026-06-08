import api from "../api";
import type { User } from "../../models/user/User";
import type { IUsersAPIService } from "./IUsersAPIService";
import type { UpdateRoleRequest } from "../../dtos/UpdateRoleRequest";
import type { UpdateUserRequest } from "../../dtos/UpdateUserRequest";

export const usersApi: IUsersAPIService = {
  getAllUsers: async function (): Promise<User[]> {
    const response = await api.get<User[]>(`/api/User`);
    return response.data;
  },
  updateUser: async function (
    id: string,
    data: UpdateUserRequest,
  ): Promise<User> {
    const response = await api.put<User>(`/api/User/${id}`, data);
    return response.data;
  },
  updateRole: async function (
    id: string,
    data: UpdateRoleRequest,
  ): Promise<User> {
    const response = await api.patch<User>(`/api/User/${id}/role`, data);
    return response.data;
  },
  deleteUser: async function (id: string): Promise<void> {
    await api.delete(`/api/User/${id}`);
  },
  getUserById: async (id: string): Promise<User> => {
    const response = await api.get<User>(`/api/User/${id}`);
    return response.data;
  },
};
