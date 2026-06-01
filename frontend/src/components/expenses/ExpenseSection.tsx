import { useState } from "react";
import { ConfirmDeleteModal } from "../ui/ConfirmDeleteModal";
import { AddExpenseModal } from "./AddExpensesModal";
import { expenseApiService } from "../../api_services/expense/ExpenseApiService";
import { CATEGORY_COLORS } from "../../enums/ExpenseCategories";
import type { ExpenseSectionProps } from "../../props/ExpenseSectionProps";

export function ExpenseSection({
  tripId,
  plannedBudget,
  expenses,
  totalExpenses,
  remainingBudget,
  onExpenseAdded,
  onExpenseDeleted,
}: ExpenseSectionProps) {
  const [isAddOpen, setIsAddOpen] = useState(false);
  const [deleteTarget, setDeleteTarget] = useState<string | null>(null);
  const [isDeleting, setIsDeleting] = useState(false);

  const spentPercent = Math.min((totalExpenses / plannedBudget) * 100, 100);

  const handleDelete = async () => {
    if (!deleteTarget) return;
    setIsDeleting(true);
    try {
      await expenseApiService.deleteExpense(deleteTarget);
      onExpenseDeleted(deleteTarget);
      setDeleteTarget(null);
    } finally {
      setIsDeleting(false);
    }
  };

  return (
    <>
      <div className="bg-white rounded-2xl border border-slate-100 shadow-sm p-5">
        <div className="flex items-center justify-between mb-4">
          <h3 className="font-semibold text-slate-800">Budget & Expenses</h3>
          <button
            onClick={() => setIsAddOpen(true)}
            className="flex items-center gap-1.5 px-3 py-1.5 rounded-lg
              bg-blue-500 hover:bg-blue-600 text-white text-xs font-medium transition-colors"
          >
            <svg
              className="w-3.5 h-3.5"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M12 4v16m8-8H4"
              />
            </svg>
            Add Expense
          </button>
        </div>

        <div className="grid grid-cols-3 gap-3 mb-4">
          <div className="text-center p-3 bg-slate-50 rounded-xl">
            <p className="text-xs text-slate-400 mb-1">Budget</p>
            <p className="font-bold text-slate-700">
              ${plannedBudget.toLocaleString()}
            </p>
          </div>
          <div className="text-center p-3 bg-red-50 rounded-xl">
            <p className="text-xs text-red-400 mb-1">Spent</p>
            <p className="font-bold text-red-600">
              ${totalExpenses.toLocaleString()}
            </p>
          </div>
          <div
            className={`text-center p-3 rounded-xl
            ${remainingBudget >= 0 ? "bg-green-50" : "bg-red-50"}`}
          >
            <p
              className={`text-xs mb-1 ${remainingBudget >= 0 ? "text-green-400" : "text-red-400"}`}
            >
              Remaining
            </p>
            <p
              className={`font-bold ${remainingBudget >= 0 ? "text-green-600" : "text-red-600"}`}
            >
              ${Math.abs(remainingBudget).toLocaleString()}
            </p>
          </div>
        </div>

        <div className="w-full bg-slate-100 rounded-full h-2 mb-4">
          <div
            className={`h-2 rounded-full transition-all duration-300
              ${spentPercent >= 100 ? "bg-red-500" : spentPercent >= 75 ? "bg-amber-400" : "bg-green-400"}`}
            style={{ width: `${spentPercent}%` }}
          />
        </div>

        {expenses.length === 0 ? (
          <p className="text-sm text-slate-400 text-center py-4">
            No expenses yet
          </p>
        ) : (
          <div className="flex flex-col gap-2">
            {expenses.map((expense) => (
              <div
                key={expense.id}
                className="flex items-center justify-between px-3 py-2.5
                  bg-slate-50 rounded-xl border border-slate-100 group"
              >
                <div className="flex items-center gap-2.5">
                  <span
                    className={`text-xs px-2 py-0.5 rounded-full border font-medium
                    ${CATEGORY_COLORS[expense.category] ?? CATEGORY_COLORS.Other}`}
                  >
                    {expense.category}
                  </span>
                  <span className="text-sm font-medium text-slate-700">
                    {expense.name}
                  </span>
                </div>
                <div className="flex items-center gap-2">
                  <span className="text-sm font-semibold text-slate-700">
                    ${expense.amount.toLocaleString()}
                  </span>
                  <button
                    onClick={() => setDeleteTarget(expense.id)}
                    className="p-1 rounded-lg text-slate-300 hover:text-red-400
                      hover:bg-red-50 transition-all opacity-0 group-hover:opacity-100"
                  >
                    <svg
                      className="w-3.5 h-3.5"
                      fill="none"
                      stroke="currentColor"
                      viewBox="0 0 24 24"
                    >
                      <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        strokeWidth={2}
                        d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"
                      />
                    </svg>
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>

      <AddExpenseModal
        isOpen={isAddOpen}
        onClose={() => setIsAddOpen(false)}
        tripId={tripId}
        onCreated={onExpenseAdded}
      />

      <ConfirmDeleteModal
        isOpen={!!deleteTarget}
        onClose={() => setDeleteTarget(null)}
        onConfirm={handleDelete}
        isLoading={isDeleting}
        title="Delete Expense"
        message="Are you sure you want to delete this expense?"
      />
    </>
  );
}
