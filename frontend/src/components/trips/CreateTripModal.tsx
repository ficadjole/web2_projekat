import { useState } from "react";
import type { CreateTripRequest } from "../../dtos/CreateTripRequest";
import { Modal } from "../ui/Modal";
import { ModalInput } from "../ui/ModalInput";
import type { CreateTripModalProps } from "../../props/CreateTripModalProps";
import { useServices } from "../../contexts/ServiceContext";

interface FormState {
  name: string;
  description: string;
  startDate: string;
  endDate: string;
  notes: string;
  plannedBudget: string;
}

interface FormErrors {
  name?: string;
  description?: string;
  startDate?: string;
  endDate?: string;
  plannedBudget?: string;
}

export function CreateTripModal({
  isOpen,
  onClose,
  onCreated,
}: CreateTripModalProps) {
  const [form, setForm] = useState<FormState>({
    name: "",
    description: "",
    startDate: "",
    endDate: "",
    plannedBudget: "",
    notes: "",
  });

  const [errors, setErrors] = useState<FormErrors>({});
  const [apiError, setApiError] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const {tripApiService} = useServices();

  const handleChange =
    (field: keyof FormState) =>
    (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
      setForm((prev) => ({ ...prev, [field]: e.target.value }));
      setErrors((prev) => ({ ...prev, [field]: "" }));
    };

  const validate = (): boolean => {
    const newErrors: FormErrors = {};
    if (!form.name.trim()) newErrors.name = "Name is required.";

    if (!form.description.trim())
      newErrors.description = "Description is required.";

    if (!form.startDate) newErrors.startDate = "Start date is required.";

    if (!form.endDate) newErrors.endDate = "End date is required.";
    else if (form.startDate && form.endDate <= form.startDate)
      newErrors.endDate = "End date must be after start date.";

    if (!form.plannedBudget) newErrors.plannedBudget = "Budget is required.";
    else if (Number(form.plannedBudget) < 0)
      newErrors.plannedBudget = "Budget cannot be negative.";

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async () => {
    if (!validate()) return;
    setIsLoading(true);
    setApiError("");
    try {
      const data: CreateTripRequest = {
        name: form.name,
        description: form.description,
        startDate: form.startDate,
        endDate: form.endDate,
        plannedBudget: Number(form.plannedBudget),
      };
      const trip = await tripApiService.createTrip(data);
      onCreated(trip);
      onClose();
      setForm({
        name: "",
        description: "",
        startDate: "",
        endDate: "",
        plannedBudget: "",
        notes: "",
      });
    } catch (err: any) {
      setApiError(err.response?.data || "Failed to create trip.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Plan a New Trip">
      <div className="flex flex-col gap-4">
        {apiError && (
          <div
            className="px-4 py-3 bg-red-50 border border-red-100 rounded-xl
            text-sm text-red-500"
          >
            {apiError}
          </div>
        )}

        <ModalInput
          label="Trip Name"
          value={form.name}
          onChange={handleChange("name")}
          error={errors.name}
          placeholder="Summer in Italy"
        />

        <ModalInput
          label="Description"
          value={form.description}
          onChange={handleChange("description")}
          error={errors.description}
          placeholder="A wonderful journey through..."
          textarea
        />

        <ModalInput
          label="Notes (optional)"
          value={form.notes}
          onChange={handleChange("notes")}
          placeholder="General notes..."
          textarea
        />

        <div className="grid grid-cols-2 gap-3">
          <ModalInput
            label="Start Date"
            type="date"
            value={form.startDate}
            onChange={handleChange("startDate")}
            error={errors.startDate}
          />
          <ModalInput
            label="End Date"
            type="date"
            value={form.endDate}
            onChange={handleChange("endDate")}
            error={errors.endDate}
          />
        </div>

        <ModalInput
          label="Planned Budget ($)"
          type="number"
          value={form.plannedBudget}
          onChange={handleChange("plannedBudget")}
          error={errors.plannedBudget}
          placeholder="2000"
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
              text-white text-sm font-medium transition-colors
              disabled:opacity-50 disabled:cursor-not-allowed"
          >
            {isLoading ? "Creating..." : "Create Trip ✈️"}
          </button>
        </div>
      </div>
    </Modal>
  );
}
