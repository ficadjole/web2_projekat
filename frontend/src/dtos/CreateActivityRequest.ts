export interface CreateActivityRequest {
  name: string;
  location: string;
  description: string;
  date: string;
  estimatedCost: number;
  status: "Planned" | "Reserved" | "Finished" | "Cancelled";
  destinationId: string;
}
