import type { Expense } from "../models/tripService/Expense";

export interface ExpenseSectionProps {
  tripId: string;
  plannedBudget: number;
  expenses: Expense[];
  totalExpenses: number;
  remainingBudget: number;
  onExpenseAdded: (expense: Expense) => void;
  onExpenseDeleted: (id: string) => void;
}
