import type { Destination } from "../models/tripService/Destination";

export interface ActivityCalendarProps {
  destinations: Destination[];
  startDate: string;
  endDate: string;
}
