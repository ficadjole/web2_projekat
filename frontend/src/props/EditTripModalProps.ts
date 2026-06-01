import type { Trip } from "../models/tripService/Trip";

export interface EditTripModalProps {
  isOpen: boolean;
  onClose: () => void;
  trip: Trip | null;
  onUpdated: (trip: Trip) => void;
}
