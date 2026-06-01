import type { CreateExpenseRequest } from "../../dtos/CreateExpenseRequest";
import type { BudgetSummary } from "../../models/tripService/BudgetSummary";
import type { Expense } from "../../models/tripService/Expense";

export interface IExpenseApiService {
  getByTripId(tripId: string): Promise<Expense[]>;
  getBudgetSummary(tripId: string): Promise<BudgetSummary>;
  createExpense(data: CreateExpenseRequest): Promise<Expense>;
  deleteExpense(id: string): Promise<void>;
}
