import type { Activity } from "../models/tripService/Activity";

export interface EditActivityModalProps {
  isOpen: boolean;
  onClose: () => void;
  activity: Activity | null;
  onUpdated: (activity: Activity) => void;
  destStartDate: string;
  destEndDate: string;
  remainingBudget: number;
}
