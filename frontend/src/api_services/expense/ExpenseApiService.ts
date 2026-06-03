import type { CreateExpenseRequest } from "../../dtos/CreateExpenseRequest";
import type { BudgetSummary } from "../../models/tripService/BudgetSummary";
import type { Expense } from "../../models/tripService/Expense";
import api from "../api";
import type { IExpenseApiService } from "./IExpenseApiService";

export const expenseApiService: IExpenseApiService = {
  getByTripId: async (tripId: string): Promise<Expense[]> => {
    const response = await api.get<Expense[]>(`/api/Expense/trip/${tripId}`);
    return response.data;
  },

  getBudgetSummary: async (tripId: string): Promise<BudgetSummary> => {
    const response = await api.get<BudgetSummary>(
      `/api/Expense/trip/${tripId}/summary`,
    );
    return response.data;
  },

  createExpense: async (data: CreateExpenseRequest): Promise<Expense> => {
    const response = await api.post<Expense>(`/api/Expense/create`, data);
    return response.data;
  },

  deleteExpense: async (id: string): Promise<void> => {
    await api.delete(`/api/Expense/${id}`);
  },
};
