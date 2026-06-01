import type { Expense } from "../models/tripService/Expense";

export interface AddExpenseModalProps {
  isOpen: boolean;
  onClose: () => void;
  tripId: string;
  onCreated: (expense: Expense) => void;
}
