import type { Activity } from "../models/tripService/Activity";
import type { Destination } from "../models/tripService/Destination";

export interface DestinationCardProps {
  destination: Destination;
  tripStartDate: string;
  tripEndDate: string;
  tripId: string;
  remainingBudget: number;
  onActivityCreated: (destinationId: string, activity: Activity) => void;
  onDestinationUpdated: (destination: Destination) => void;
  onActivityUpdated: (destinationId: string, activity: Activity) => void;
  onDestinationDeleted: (destinationId: string) => void;
  onActivityDeleted: (destinationId: string, activityId: string) => void;
}
