import type { CreateActivityRequest } from "../../dtos/CreateActivityRequest";
import type { UpdateActivityRequest } from "../../dtos/UpdateActivityRequest";
import type { Activity } from "../../models/tripService/Activity";
import api from "../api";
import type { IActivityApiService } from "./IActivityApiService";

export const activityApiService: IActivityApiService = {
  createActivity: async (data: CreateActivityRequest): Promise<Activity> => {
    const response = await api.post<Activity>(`/api/Activity/create`, data);
    return response.data;
  },
  updateActivity: async (
    data: UpdateActivityRequest,
    id: string,
  ): Promise<Activity> => {
    const response = await api.put<Activity>(`/api/Activity/${id}`, data);
    return response.data;
  },
  deleteActivity: async (id: string): Promise<void> => {
    await api.delete(`/api/Activity/${id}`);
  },
};
