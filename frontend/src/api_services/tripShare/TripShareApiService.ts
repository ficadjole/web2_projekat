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

  downloadReport: async (tripId: string): Promise<void> => {
    const response = await api.get(`/api/TripShare/${tripId}/report`, {
      responseType: "blob",
    });

    const url = window.URL.createObjectURL(new Blob([response.data]));

    const link = document.createElement("a");
    link.href = url;
    link.setAttribute("download", `trip-report-${tripId}.pdf`);
    document.body.appendChild(link);
    link.click();
    link.remove();
    window.URL.revokeObjectURL(url);
  },
};
