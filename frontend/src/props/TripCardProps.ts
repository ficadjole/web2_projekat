import type { Trip } from "../models/tripService/Trip";

export interface TripCardProps {
  trip: Trip;
  onUpdated: (trip: Trip) => void;
  onDeleted: (id: string) => void;
}
