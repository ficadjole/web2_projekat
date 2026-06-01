import type { Expense } from "./Expense";

export interface BudgetSummary {
  plannedBudget: number;
  totalExpenses: number;
  remainingBudget: number;
  expenses: Expense[];
}
