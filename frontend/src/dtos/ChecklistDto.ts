import type { ChecklistItem } from "../models/checklistService/ChecklistItem";

export interface ChecklistDto {
  id: string;
  tripId: string;
  items: ChecklistItem[];
}
