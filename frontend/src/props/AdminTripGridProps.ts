import type { Trip } from "../models/tripService/Trip";

export interface AdminTripGridProps {
  trips: Trip[];
  onUpdated: (trip: Trip) => void;
  onDeleted: (id: string) => void;
}
