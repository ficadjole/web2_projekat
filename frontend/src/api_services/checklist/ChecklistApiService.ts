import type { ChecklistDto } from "../../dtos/ChecklistDto";
import type { ChecklistItem } from "../../models/checklistService/ChecklistItem";
import api from "../api";
import type { IChecklistApiService } from "./IChecklistApiService";

const BASE = import.meta.env.VITE_API_URL + "Checklist";

export const checklistApiService: IChecklistApiService = {
  getByTripId: async (tripId: string): Promise<ChecklistDto> => {
    const response = await api.get<ChecklistDto>(`${BASE}/trip/${tripId}`);
    return response.data;
  },

  addItem: async (data): Promise<ChecklistItem> => {
    const response = await api.post<ChecklistItem>(`${BASE}/items/add`, data);
    return response.data;
  },

  toggleItem: async (
    tripId: string,
    itemId: string,
  ): Promise<ChecklistItem> => {
    const response = await api.patch<ChecklistItem>(
      `${BASE}/trip/${tripId}/items/${itemId}/toggle`,
    );
    return response.data;
  },

  deleteItem: async (tripId: string, itemId: string): Promise<void> => {
    await api.delete(`${BASE}/trip/${tripId}/items/${itemId}`);
  },
};
