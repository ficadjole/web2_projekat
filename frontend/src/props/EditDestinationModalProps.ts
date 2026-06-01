import type { Destination } from "../models/tripService/Destination";

export interface EditDestinationModalProps {
  isOpen: boolean;
  onClose: () => void;
  destination: Destination | null;
  onUpdated: (destination: Destination) => void;
  tripStartDate: string;
  tripEndDate: string;
}
