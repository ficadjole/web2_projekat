import { useState } from "react";
import { Modal } from "../ui/Modal";
import { ModalInput } from "../ui/ModalInput";
import { activityApiService } from "../../api_services/acitivity/ActivityApiService";
import type { CreateActivityRequest } from "../../dtos/CreateActivityRequest";
import type { CreateActivityModalProps } from "../../props/CreateActivityModal";

interface FormState {
  name: string;
  location: string;
  description: string;
  date: string;
  estimatedCost: string;
  status: "Planned" | "Reserved" | "Finished" | "Cancelled";
}

interface FormErrors {
  name?: string;
  location?: string;
  date?: string;
  estimatedCost?: string;
}

const CreateActivityModal = ({
  isOpen,
  onClose,
  destinationId,
  onCreated,
  destEndDate,
  destStartDate,
  remainingBudget,
}: CreateActivityModalProps) => {
  const [form, setForm] = useState<FormState>({
    name: "",
    location: "",
    description: "",
    date: "",
    estimatedCost: "",
    status: "Planned",
  });

  const [errors, setErrors] = useState<FormErrors>({});
  const [apiError, setApiError] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const handleChange =
    (field: keyof FormState) =>
    (
      e: React.ChangeEvent<
        HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement
      >,
    ) => {
      setForm((prev) => ({ ...prev, [field]: e.target.value }));
      setErrors((prev) => ({ ...prev, [field]: "" }));
    };

  const validate = (): boolean => {
    const newErrors: FormErrors = {};
    const activityDateOnly = form.date.split("T")[0];
    const destStartOnly = destStartDate.split("T")[0];
    const destEndOnly = destEndDate.split("T")[0];

    const cost = Number(form.estimatedCost) || 0;
    if (cost > remainingBudget) {
      newErrors.estimatedCost = `Cost exceeds remaining budget of $${remainingBudget}.`;
    }
    
    if (!form.name.trim()) newErrors.name = "Name is required.";

    if (!form.location.trim()) newErrors.location = "Location is required.";

    if (!form.date) newErrors.date = "Date is required.";

    if (form.estimatedCost && Number(form.estimatedCost) < 0)
      newErrors.estimatedCost = "Cost cannot be negative.";

    if (activityDateOnly < destStartOnly || activityDateOnly > destEndOnly) {
      newErrors.date = "Activity date must be within destination dates.";
    }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async () => {
    if (!validate()) return;
    setIsLoading(true);
    setApiError("");
    try {
      const data: CreateActivityRequest = {
        name: form.name,
        location: form.location,
        description: form.description,
        date: form.date,
        estimatedCost: Number(form.estimatedCost) || 0,
        status: form.status,
        destinationId,
      };
      const activity = await activityApiService.createActivity(data);
      onCreated(activity);
      onClose();
      setForm({
        name: "",
        location: "",
        description: "",
        date: "",
        estimatedCost: "",
        status: "Planned",
      });
    } catch (err: any) {
      setApiError(err.response?.data || "Failed to create activity.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Add Activity">
      <div className="flex flex-col gap-4">
        {apiError && (
          <div className="px-4 py-3 bg-red-50 border border-red-100 rounded-xl text-sm text-red-500">
            {apiError}
          </div>
        )}

        <ModalInput
          label="Activity Name"
          value={form.name}
          onChange={handleChange("name")}
          error={errors.name}
          placeholder="Visit Colosseum"
        />

        <ModalInput
          label="Location"
          value={form.location}
          onChange={handleChange("location")}
          error={errors.location}
          placeholder="Piazza del Colosseo, Rome"
        />

        <ModalInput
          label="Description (optional)"
          value={form.description}
          onChange={handleChange("description")}
          placeholder="Guided tour of the ancient amphitheatre..."
          textarea
        />

        <div className="grid grid-cols-2 gap-3">
          <ModalInput
            label="Date"
            type="datetime-local"
            value={form.date}
            onChange={handleChange("date")}
            error={errors.date}
          />
          <ModalInput
            label="Estimated Cost ($)"
            type="number"
            value={form.estimatedCost}
            onChange={handleChange("estimatedCost")}
            error={errors.estimatedCost}
            placeholder="0"
          />
        </div>

        <div className="flex flex-col gap-1">
          <label className="text-sm font-medium text-slate-600">Status</label>
          <select
            value={form.status}
            onChange={handleChange("status") as any}
            className="w-full px-4 py-2.5 rounded-xl border border-slate-200 text-slate-800
              text-sm focus:outline-none focus:ring-2 focus:ring-blue-300
              focus:border-blue-300 transition-all duration-150 bg-white"
          >
            <option value="Planned">Planned</option>
            <option value="Reserved">Reserved</option>
            <option value="Finished">Finished</option>
            <option value="Cancelled">Cancelled</option>
          </select>
        </div>

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
            {isLoading ? "Adding..." : "Add Activity 🎯"}
          </button>
        </div>
      </div>
    </Modal>
  );
};

export default CreateActivityModal;
