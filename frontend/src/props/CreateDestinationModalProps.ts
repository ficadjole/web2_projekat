import type { Destination } from "../models/tripService/Destination";

export interface CreateDestinationModalProps {
  isOpen: boolean;
  onClose: () => void;
  tripId: string;
  onCreated: (destination: Destination) => void;
  tripStartDate: string;
  tripEndDate: string;
}
