import type { Activity } from "./Activity";

export interface Destination {
  id: string;
  name: string;
  description: string;
  notes: string;
  location: string;
  arrivingDate: string;
  leavingDate: string;
  tripId: string;
  activities: Activity[];
}
