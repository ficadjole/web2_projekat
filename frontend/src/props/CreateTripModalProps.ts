import type { Trip } from "../models/tripService/Trip";

export interface CreateTripModalProps {
  isOpen: boolean;
  onClose: () => void;
  onCreated: (trip: Trip) => void;
}
