import type { CreateTripRequest } from "../../dtos/CreateTripRequest";
import type { UpdateTripRequest } from "../../dtos/UpdateTripRequest";
import type { Trip } from "../../models/tripService/Trip";
import type { TripDetails } from "../../models/tripService/TripDetails";
import api from "../api";
import type { ITripApiService } from "./ITripApiService";

export const tripApiService: ITripApiService = {
  getAllTrips: async (): Promise<Trip[]> => {
    const response = await api.get<Trip[]>(`/api/Trip`);
    return response.data;
  },

  getTripById: async (id: string): Promise<Trip> => {
    const response = await api.get<Trip>(`/api/Trip/${id}`);
    return response.data;
  },

  getTripWithDetails: async (id: string): Promise<TripDetails> => {
    const response = await api.get<TripDetails>(`/api/Trip/${id}/details`);
    return response.data;
  },

  createTrip: async (data: CreateTripRequest): Promise<Trip> => {
    const response = await api.post<Trip>(`/api/Trip/create`, data);
    return response.data;
  },

  updateTrip: async (id: string, data: UpdateTripRequest): Promise<Trip> => {
    const response = await api.put<Trip>(`/api/Trip/${id}`, data);
    return response.data;
  },

  deleteTrip: async (id: string): Promise<void> => {
    await api.delete(`/api/Trip/${id}`);
  },
};
