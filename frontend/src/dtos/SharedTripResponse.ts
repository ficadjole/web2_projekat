import type { TripDetails } from "../models/tripService/TripDetails";

export interface SharedTripResponse {
  trip: TripDetails;
  accessType: "View" | "Edit";
}
