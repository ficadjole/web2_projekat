export interface TripShareResponse {
  id: string;
  tripId: string;
  token: string;
  accessType: "View" | "Edit";
  expiresAt: string;
  shareUrl: string;
}
