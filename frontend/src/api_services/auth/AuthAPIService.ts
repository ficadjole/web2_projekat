import type { AuthResponse } from "../../types/auth/AuthResponse";
import type { IAuthAPIService } from "./IAuthAPIService";
import type { LoginRequest } from "../../dtos/LoginRequest";
import api from "../api";
import type { RegisterRequest } from "../../dtos/RegisterRequest";
import { removeItem } from "../../helpers/local_storage";

export const authApi: IAuthAPIService = {
  async login(loginData: LoginRequest) {
    const response = await api.post<AuthResponse>(`/api/Auth/login`, loginData);
    return response.data;
  },
  async register(registerData: RegisterRequest) {
    const response = await api.post<AuthResponse>(
      `/api/Auth/registration`,
      registerData,
    );
    return response.data;
  },

  logout() {
    removeItem("token");
  },
};
