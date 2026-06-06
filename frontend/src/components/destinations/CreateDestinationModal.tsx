import { Modal } from "../ui/Modal";
import { ModalInput } from "../ui/ModalInput";
import type { CreateDestinationModalProps } from "../../props/CreateDestinationModalProps";
import { useDestinationForm } from "../../hooks/destination/useDestinationForm";

const defaultInitialState = {
  name: "",
  description: "",
  notes: "",
  location: "",
  arrivingDate: "",
  leavingDate: "",
};

export function CreateDestinationModal({
  isOpen,
  onClose,
  tripId,
  onCreated,
  tripEndDate,
  tripStartDate,
}: CreateDestinationModalProps) {
  const { form, errors, apiError, isLoading, handleChange, handleSubmit } =
    useDestinationForm({
      initialState: defaultInitialState,
      tripStartDate,
      tripEndDate,
      onClose,
      onSubmitSuccess: onCreated,
      mode: "create",
      tripId,
    });

  const formattedMin = tripStartDate ? tripStartDate.split("T")[0] : "";
  const formattedMax = tripEndDate ? tripEndDate.split("T")[0] : "";

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
            min={formattedMin}
            max={formattedMax}
          />
          <ModalInput
            label="Leaving Date"
            type="date"
            value={form.leavingDate}
            onChange={handleChange("leavingDate")}
            error={errors.leavingDate}
            min={formattedMin}
            max={formattedMax}
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
