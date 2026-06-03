import type { ChecklistDto } from "../../dtos/ChecklistDto";
import type { ChecklistItem } from "../../models/checklistService/ChecklistItem";
import api from "../api";
import type { IChecklistApiService } from "./IChecklistApiService";

export const checklistApiService: IChecklistApiService = {
  getByTripId: async (tripId: string): Promise<ChecklistDto> => {
    const response = await api.get<ChecklistDto>(
      `/api/Checklist/trip/${tripId}`,
    );
    return response.data;
  },

  addItem: async (data): Promise<ChecklistItem> => {
    const response = await api.post<ChecklistItem>(
      `/api/Checklist/items/add`,
      data,
    );
    return response.data;
  },

  toggleItem: async (
    tripId: string,
    itemId: string,
  ): Promise<ChecklistItem> => {
    const response = await api.patch<ChecklistItem>(
      `/api/Checklist/trip/${tripId}/items/${itemId}/toggle`,
    );
    return response.data;
  },

  deleteItem: async (tripId: string, itemId: string): Promise<void> => {
    await api.delete(`/api/Checklist/trip/${tripId}/items/${itemId}`);
  },
};
