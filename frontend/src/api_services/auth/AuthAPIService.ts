import type { AuthResponse } from "../../types/auth/AuthResponse";
import type { IAuthAPIService } from "./IAuthAPIService";
import type { LoginRequest } from "../../dtos/LoginRequest";
import api from "../api";
import type { RegisterRequest } from "../../dtos/RegisterRequest";
import { removeItem } from "../../helpers/local_storage";

const BASE = import.meta.env.VITE_API_URL + "Auth";

export const authApi: IAuthAPIService = {
  async login(loginData: LoginRequest) {
    const response = await api.post<AuthResponse>(`${BASE}/login`, loginData);
    return response.data;
  },
  async register(registerData: RegisterRequest) {
    const response = await api.post<AuthResponse>(
      `${BASE}/registration`,
      registerData,
    );
    return response.data;
  },

  logout() {
    removeItem("token");
  },
};
