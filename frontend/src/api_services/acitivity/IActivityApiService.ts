import type { CreateActivityRequest } from "../../dtos/CreateActivityRequest";
import type { UpdateActivityRequest } from "../../dtos/UpdateActivityRequest";
import type { Activity } from "../../models/tripService/Activity";

export interface IActivityApiService {
  createActivity(data: CreateActivityRequest): Promise<Activity>;
  updateActivity(data: UpdateActivityRequest, id: string): Promise<Activity>;
  deleteActivity(id: string): Promise<void>;
}
