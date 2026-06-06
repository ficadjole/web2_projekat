import { Modal } from "../ui/Modal";
import { ModalInput } from "../ui/ModalInput";
import type { CreateActivityModalProps } from "../../props/CreateActivityModal";
import { useActivityForm } from "../../hooks/activity/useActivityForm";

const defaultInitialState = {
  name: "",
  location: "",
  description: "",
  date: "",
  estimatedCost: "",
  status: "Planned" as const,
};

const CreateActivityModal = ({
  isOpen,
  onClose,
  destinationId,
  onCreated,
  destEndDate,
  destStartDate,
  remainingBudget,
}: CreateActivityModalProps) => {
  const { form, errors, apiError, isLoading, handleChange, handleSubmit } =
    useActivityForm({
      initialState: defaultInitialState,
      remainingBudget,
      destStartDate,
      destEndDate,
      onClose,
      onSubmitSuccess: onCreated,
      mode: "create",
      destinationId: destinationId,
    });

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
            min={destStartDate}
            max={destEndDate}
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
            onClick={() => handleSubmit(0)}
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
