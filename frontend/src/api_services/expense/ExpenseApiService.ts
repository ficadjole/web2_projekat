import type { CreateExpenseRequest } from "../../dtos/CreateExpenseRequest";
import type { BudgetSummary } from "../../models/tripService/BudgetSummary";
import type { Expense } from "../../models/tripService/Expense";
import api from "../api";
import type { IExpenseApiService } from "./IExpenseApiService";

const BASE = import.meta.env.VITE_API_URL + "Expense";

export const expenseApiService: IExpenseApiService = {
  getByTripId: async (tripId: string): Promise<Expense[]> => {
    const response = await api.get<Expense[]>(`${BASE}/trip/${tripId}`);
    return response.data;
  },

  getBudgetSummary: async (tripId: string): Promise<BudgetSummary> => {
    const response = await api.get<BudgetSummary>(
      `${BASE}/trip/${tripId}/summary`,
    );
    return response.data;
  },

  createExpense: async (data: CreateExpenseRequest): Promise<Expense> => {
    const response = await api.post<Expense>(`${BASE}/create`, data);
    return response.data;
  },

  deleteExpense: async (id: string): Promise<void> => {
    await api.delete(`${BASE}/${id}`);
  },
};
