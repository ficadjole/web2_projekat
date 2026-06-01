import { useEffect, useState } from "react";
import { tripApiService } from "../../api_services/trip/TripApiService";
import { Modal } from "../ui/Modal";
import { ModalInput } from "../ui/ModalInput";
import type { EditTripModalProps } from "../../props/EditTripModalProps";

export function EditTripModal({
  isOpen,
  onClose,
  trip,
  onUpdated,
}: EditTripModalProps) {
  const [form, setForm] = useState({
    name: "",
    description: "",
    notes: "",
    startDate: "",
    endDate: "",
    plannedBudget: "",
  });
  const [errors, setErrors] = useState<Record<string, string>>({});
  const [apiError, setApiError] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    if (trip) {
      setForm({
        name: trip.name,
        description: trip.description,
        notes: trip.notes ?? "",
        startDate: trip.startDate.split("T")[0],
        endDate: trip.endDate.split("T")[0],
        plannedBudget: trip.plannedBudget.toString(),
      });
    }
  }, [trip]);

  const handleChange =
    (field: string) =>
    (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
      setForm((prev) => ({ ...prev, [field]: e.target.value }));
      setErrors((prev) => ({ ...prev, [field]: "" }));
    };

  const validate = () => {
    const newErrors: Record<string, string> = {};

    if (!form.name.trim()) newErrors.name = "Name is required.";

    if (!form.description.trim())
      newErrors.description = "Description is required.";

    if (!form.startDate) newErrors.startDate = "Start date is required.";

    if (!form.endDate) newErrors.endDate = "End date is required.";
    else if (form.endDate <= form.startDate)
      newErrors.endDate = "End date must be after start date.";

    if (Number(form.plannedBudget) < 0)
      newErrors.plannedBudget = "Budget cannot be negative.";

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async () => {
    if (!validate() || !trip) return;
    setIsLoading(true);
    setApiError("");
    try {
      const updated = await tripApiService.updateTrip(trip.id, {
        name: form.name,
        description: form.description,
        notes: form.notes,
        startDate: form.startDate,
        endDate: form.endDate,
        plannedBudget: Number(form.plannedBudget),
      });
      onUpdated(updated);
      onClose();
    } catch (err: any) {
      setApiError(err.response?.data || "Failed to update trip.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Edit Trip">
      <div className="flex flex-col gap-4">
        {apiError && (
          <div className="px-4 py-3 bg-red-50 border border-red-100 rounded-xl text-sm text-red-500">
            {apiError}
          </div>
        )}
        <ModalInput
          label="Trip Name"
          value={form.name}
          onChange={handleChange("name")}
          error={errors.name}
        />
        <ModalInput
          label="Description"
          value={form.description}
          onChange={handleChange("description")}
          error={errors.description}
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
            {isLoading ? "Saving..." : "Save Changes"}
          </button>
        </div>
      </div>
    </Modal>
  );
}
