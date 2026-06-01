import type { CreateActivityRequest } from "../../dtos/CreateActivityRequest";
import type { UpdateActivityRequest } from "../../dtos/UpdateActivityRequest";
import type { Activity } from "../../models/tripService/Activity";
import api from "../api";
import type { IActivityApiService } from "./IActivityApiService";

const BASE = import.meta.env.VITE_API_URL + "Activity";

export const activityApiService: IActivityApiService = {
  createActivity: async (data: CreateActivityRequest): Promise<Activity> => {
    const response = await api.post<Activity>(`${BASE}/create`, data);
    return response.data;
  },
  updateActivity: async (
    data: UpdateActivityRequest,
    id: string,
  ): Promise<Activity> => {
    const response = await api.put<Activity>(`${BASE}/${id}`, data);
    return response.data;
  },
  deleteActivity: async (id: string): Promise<void> => {
    await api.delete(`${BASE}/${id}`);
  },
};
