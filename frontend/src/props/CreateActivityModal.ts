import type { Activity } from "../models/tripService/Activity";

export interface CreateActivityModalProps {
  isOpen: boolean;
  onClose: () => void;
  destinationId: string;
  onCreated: (activity: Activity) => void;
  destStartDate: string;
  destEndDate: string;
  remainingBudget: number;
}
