import type { CreateTripRequest } from "../../dtos/CreateTripRequest";
import type { UpdateTripRequest } from "../../dtos/UpdateTripRequest";
import type { Trip } from "../../models/tripService/Trip";
import type { TripDetails } from "../../models/tripService/TripDetails";

export interface ITripApiService {
  getAllTrips(): Promise<Trip[]>;
  getTripById(id: string): Promise<Trip>;
  getTripWithDetails(id: string): Promise<TripDetails>;
  createTrip(data: CreateTripRequest): Promise<Trip>;
  updateTrip(id: string, data: UpdateTripRequest): Promise<Trip>;
  deleteTrip(id: string): Promise<void>;
}
