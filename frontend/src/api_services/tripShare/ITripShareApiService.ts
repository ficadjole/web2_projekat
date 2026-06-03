import type { CreateShareRequest } from "../../dtos/CreateShareData";
import type { SharedTripResponse } from "../../dtos/SharedTripResponse";
import type { TripShareResponse } from "../../dtos/TripShareResponse";

export interface ITripShareApiService {
  createShare(data: CreateShareRequest): Promise<TripShareResponse>;
  getSharedTrip(token: string): Promise<SharedTripResponse>;
  getSharesByTripId(tripId: string): Promise<TripShareResponse[]>;
  revokeShare(id: string): Promise<void>;
}
