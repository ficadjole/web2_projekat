export interface CreateExpenseRequest {
  name: string;
  category: string;
  amount: number;
  description: string;
  tripId: string;
}
