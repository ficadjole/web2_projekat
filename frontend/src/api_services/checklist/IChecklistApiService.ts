import type { ChecklistDto } from "../../dtos/ChecklistDto";
import type { ChecklistItem } from "../../models/checklistService/ChecklistItem";

export interface IChecklistApiService {
  getByTripId(tripId: string): Promise<ChecklistDto>;
  addItem(data: { name: string; tripId: string }): Promise<ChecklistItem>;
  toggleItem(tripId: string, itemId: string): Promise<ChecklistItem>;
  deleteItem(tripId: string, itemId: string): Promise<void>;
}
