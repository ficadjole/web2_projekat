import { useState } from "react";
import { destinationApiService } from "../../api_services/destination/DestinationApiService";
import type { CreateDestinationData } from "../../dtos/CreateDestinationData";
import { Modal } from "../ui/Modal";
import { ModalInput } from "../ui/ModalInput";
import type { CreateDestinationModalProps } from "../../props/CreateDestinationModalProps";

interface FormState {
  name: string;
  description: string;
  notes: string;
  location: string;
  arrivingDate: string;
  leavingDate: string;
}

interface FormErrors {
  name?: string;
  description?: string;
  location?: string;
  arrivingDate?: string;
  leavingDate?: string;
}

export function CreateDestinationModal({
  isOpen,
  onClose,
  tripId,
  onCreated,
  tripEndDate,
  tripStartDate,
}: CreateDestinationModalProps) {
  const [form, setForm] = useState<FormState>({
    name: "",
    description: "",
    notes: "",
    location: "",
    arrivingDate: "",
    leavingDate: "",
  });

  const [errors, setErrors] = useState<FormErrors>({});
  const [apiError, setApiError] = useState("");
  const [isLoading, setIsLoading] = useState(false);

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

    if (!form.location.trim()) newErrors.location = "Location is required.";

    if (!form.arrivingDate)
      newErrors.arrivingDate = "Arriving date is required.";

    if (!form.leavingDate) newErrors.leavingDate = "Leaving date is required.";
    else if (form.arrivingDate && form.leavingDate <= form.arrivingDate)
      newErrors.leavingDate = "Leaving date must be after arriving date.";

    if (form.arrivingDate < tripStartDate.split("T")[0]) {
      newErrors.arrivingDate = "Arriving date cannot be before trip starts.";
    }
    if (form.leavingDate > tripEndDate.split("T")[0]) {
      newErrors.leavingDate = "Leaving date cannot be after trip ends.";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async () => {
    if (!validate()) return;
    setIsLoading(true);
    setApiError("");
    try {
      const data: CreateDestinationData = {
        ...form,
        tripId,
      };
      const destination = await destinationApiService.createDestination(data);
      onCreated(destination);
      onClose();
      setForm({
        name: "",
        description: "",
        notes: "",
        location: "",
        arrivingDate: "",
        leavingDate: "",
      });
    } catch (err: any) {
      setApiError(err.response?.data || "Failed to create destination.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Add Destination">
      <div className="flex flex-col gap-4">
        {apiError && (
          <div className="px-4 py-3 bg-red-50 border border-red-100 rounded-xl text-sm text-red-500">
            {apiError}
          </div>
        )}

        <ModalInput
          label="Destination Name"
          value={form.name}
          onChange={handleChange("name")}
          error={errors.name}
          placeholder="Rome, Italy"
        />

        <ModalInput
          label="Location"
          value={form.location}
          onChange={handleChange("location")}
          error={errors.location}
          placeholder="Rome, Lazio, Italy"
        />

        <ModalInput
          label="Description"
          value={form.description}
          onChange={handleChange("description")}
          error={errors.description}
          placeholder="The eternal city..."
          textarea
        />

        <ModalInput
          label="Notes (optional)"
          value={form.notes}
          onChange={handleChange("notes")}
          placeholder="Book hotel in advance..."
          textarea
        />

        <div className="grid grid-cols-2 gap-3">
          <ModalInput
            label="Arriving Date"
            type="date"
            value={form.arrivingDate}
            onChange={handleChange("arrivingDate")}
            error={errors.arrivingDate}
          />
          <ModalInput
            label="Leaving Date"
            type="date"
            value={form.leavingDate}
            onChange={handleChange("leavingDate")}
            error={errors.leavingDate}
          />
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
            {isLoading ? "Adding..." : "Add Destination 📍"}
          </button>
        </div>
      </div>
    </Modal>
  );
}
