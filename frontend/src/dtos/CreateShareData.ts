export interface CreateShareRequest {
  tripId: string;
  accessType: "View" | "Edit";
  expiresInDays: number;
  email: string;
}
