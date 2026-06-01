import type { Trip } from "../models/tripService/Trip";

export interface TripListProps {
  trips: Trip[];
  onAddTrip: () => void;
  onUpdated: (trip: Trip) => void;
  onDeleted: (id: string) => void;
}
