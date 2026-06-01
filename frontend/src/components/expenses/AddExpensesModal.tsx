import { useState } from "react";
import { expenseApiService } from "../../api_services/expense/ExpenseApiService";
import { Modal } from "../ui/Modal";
import { ModalInput } from "../ui/ModalInput";
import type { AddExpenseModalProps } from "../../props/AddExpenseModalProps";
import { CATEGORIES } from "../../enums/ExpenseCategories";

export function AddExpenseModal({
  isOpen,
  onClose,
  tripId,
  onCreated,
}: AddExpenseModalProps) {
  const [form, setForm] = useState({
    name: "",
    category: "Other",
    amount: "",
    description: "",
  });
  const [errors, setErrors] = useState<Record<string, string>>({});
  const [apiError, setApiError] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const handleChange =
    (field: string) =>
    (
      e: React.ChangeEvent<
        HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement
      >,
    ) => {
      setForm((prev) => ({ ...prev, [field]: e.target.value }));
      setErrors((prev) => ({ ...prev, [field]: "" }));
    };

  const validate = () => {
    const newErrors: Record<string, string> = {};
    if (!form.name.trim()) newErrors.name = "Name is required.";
    if (!form.amount || Number(form.amount) <= 0)
      newErrors.amount = "Amount must be greater than zero.";
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async () => {
    if (!validate()) return;
    setIsLoading(true);
    setApiError("");
    try {
      const expense = await expenseApiService.createExpense({
        name: form.name,
        category: form.category,
        amount: Number(form.amount),
        description: form.description,
        tripId,
      });
      onCreated(expense);
      onClose();
      setForm({ name: "", category: "Other", amount: "", description: "" });
    } catch (err: any) {
      setApiError(err.response?.data || "Failed to add expense.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Add Expense">
      <div className="flex flex-col gap-4">
        {apiError && (
          <div className="px-4 py-3 bg-red-50 border border-red-100 rounded-xl text-sm text-red-500">
            {apiError}
          </div>
        )}
        <ModalInput
          label="Name"
          value={form.name}
          onChange={handleChange("name")}
          error={errors.name}
          placeholder="Flight tickets"
        />
        <div className="flex flex-col gap-1">
          <label className="text-sm font-medium text-slate-600">Category</label>
          <select
            value={form.category}
            onChange={handleChange("category") as any}
            className="w-full px-4 py-2.5 rounded-xl border border-slate-200 text-slate-800
              text-sm focus:outline-none focus:ring-2 focus:ring-blue-300 bg-white"
          >
            {CATEGORIES.map((c) => (
              <option key={c} value={c}>
                {c}
              </option>
            ))}
          </select>
        </div>
        <ModalInput
          label="Amount ($)"
          type="number"
          value={form.amount}
          onChange={handleChange("amount")}
          error={errors.amount}
          placeholder="150"
        />
        <ModalInput
          label="Description (optional)"
          value={form.description}
          onChange={handleChange("description")}
          textarea
        />
        <div className="flex gap-3 pt-2">
          <button
            onClick={onClose}
            className="flex-1 py-2.5 rounded-xl border border-slate-200 text-slate-600
              hover:bg-slate-50 transition-colors text-sm font-medium"
          >
            Cancel
          </button>
          <button
            onClick={handleSubmit}
            disabled={isLoading}
            className="flex-1 py-2.5 rounded-xl bg-blue-500 hover:bg-blue-600
              text-white text-sm font-medium transition-colors disabled:opacity-50"
          >
            {isLoading ? "Adding..." : "Add Expense 💰"}
          </button>
        </div>
      </div>
    </Modal>
  );
}
