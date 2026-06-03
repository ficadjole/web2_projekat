import type { CreateShareRequest } from "../../dtos/CreateShareData";
import type { SharedTripResponse } from "../../dtos/SharedTripResponse";
import type { TripShareResponse } from "../../dtos/TripShareResponse";
import api from "../api";
import type { ITripShareApiService } from "./ITripShareApiService";

export const tripShareApiService: ITripShareApiService = {
  createShare: async (data: CreateShareRequest): Promise<TripShareResponse> => {
    const response = await api.post<TripShareResponse>(
      `/api/TripShare/create`,
      data,
    );
    return response.data;
  },

  getSharedTrip: async (token: string): Promise<SharedTripResponse> => {
    const response = await api.get<SharedTripResponse>(
      `/api/TripShare/${token}`,
    );
    return response.data;
  },

  getSharesByTripId: async (tripId: string): Promise<TripShareResponse[]> => {
    const response = await api.get<TripShareResponse[]>(
      `/api/TripShare/trip/${tripId}`,
    );
    return response.data;
  },

  revokeShare: async (id: string): Promise<void> => {
    await api.delete(`/api/TripShare/${id}`);
  },
};
