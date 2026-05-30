import type { LoginRequest } from "../../dtos/LoginRequest";
import type { RegisterRequest } from "../../dtos/RegisterRequest";
import type { AuthResponse } from "../../types/auth/AuthResponse";

export interface IAuthAPIService {
  login(loginData: LoginRequest): Promise<AuthResponse>;
  register(registerData: RegisterRequest): Promise<AuthResponse>;
  logout(): void;
}
