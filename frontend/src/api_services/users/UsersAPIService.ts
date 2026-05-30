import api from "../api";
import type { User } from "../../models/user/User";
import type { IUsersAPIService } from "./IUsersAPIService";
import type { UpdateRoleRequest } from "../../dtos/UpdateRoleRequest";
import type { UpdateUserRequest } from "../../dtos/UpdateUserRequest";

const BASE = import.meta.env.VITE_API_URL + "User";

export const usersApi: IUsersAPIService = {
  getAllUsers: async function (): Promise<User[]> {
    const response = await api.get<User[]>(`${BASE}`);
    return response.data;
  },
  updateUser: async function (
    id: string,
    data: UpdateUserRequest,
  ): Promise<User> {
    const response = await api.put<User>(`${BASE}/${id}`, data);
    return response.data;
  },
  updateRole: async function (
    id: string,
    data: UpdateRoleRequest,
  ): Promise<User> {
    const response = await api.patch<User>(`${BASE}/${id}/role`, data);
    return response.data;
  },
  deleteUser: async function (id: string): Promise<void> {
    await api.delete(`${BASE}/${id}`);
  },
};
