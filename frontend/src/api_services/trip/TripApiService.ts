import type { CreateTripRequest } from "../../dtos/CreateTripRequest";
import type { UpdateTripRequest } from "../../dtos/UpdateTripRequest";
import type { Trip } from "../../models/tripService/Trip";
import type { TripDetails } from "../../models/tripService/TripDetails";
import api from "../api";
import type { ITripApiService } from "./ITripApiService";

const BASE = import.meta.env.VITE_API_URL + "Trip";

export const tripApiService: ITripApiService = {
  getAllTrips: async (): Promise<Trip[]> => {
    const response = await api.get<Trip[]>(`${BASE}`);
    return response.data;
  },

  getTripById: async (id: string): Promise<Trip> => {
    const response = await api.get<Trip>(`${BASE}/${id}`);
    return response.data;
  },

  getTripWithDetails: async (id: string): Promise<TripDetails> => {
    const response = await api.get<TripDetails>(`${BASE}/${id}/details`);
    return response.data;
  },

  createTrip: async (data: CreateTripRequest): Promise<Trip> => {
    const response = await api.post<Trip>(`${BASE}/create`, data);
    return response.data;
  },

  updateTrip: async (id: string, data: UpdateTripRequest): Promise<Trip> => {
    const response = await api.put<Trip>(`${BASE}/${id}`, data);
    return response.data;
  },

  deleteTrip: async (id: string): Promise<void> => {
    await api.delete(`${BASE}/${id}`);
  },
};
