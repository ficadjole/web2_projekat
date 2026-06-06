export interface FormState {
  name: string;
  location: string;
  description: string;
  date: string;
  estimatedCost: string;
  status: "Planned" | "Reserved" | "Finished" | "Cancelled";
}
