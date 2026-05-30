import type { Destination } from "./Destination";
import type { Expense } from "./Expense";
import type { Trip } from "./Trip";

export interface TripDetails extends Trip {
  destinations: Destination[];
  expenses: Expense[];
  totalExpenses: number;
  remainingBudget: number;
}
