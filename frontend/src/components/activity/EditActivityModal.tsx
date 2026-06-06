import { Modal } from "../ui/Modal";
import { ModalInput } from "../ui/ModalInput";
import type { EditActivityModalProps } from "../../props/EditActivityModalProps";
import { useActivityForm } from "../../hooks/activity/useActivityForm";

export function EditActivityModal({
  isOpen,
  onClose,
  activity,
  onUpdated,
  destEndDate,
  destStartDate,
  remainingBudget,
}: EditActivityModalProps) {
  const currentActivityState = {
    name: activity?.name || "",
    location: activity?.location || "",
    description: activity?.description || "",
    date: activity?.date ? activity.date.substring(0, 16) : "",
    estimatedCost: activity?.estimatedCost?.toString() || "",
    status: activity?.status || "Planned",
  };

  const { form, errors, apiError, isLoading, handleChange, handleSubmit } =
    useActivityForm({
      initialState: currentActivityState,
      remainingBudget,
      destStartDate,
      destEndDate,
      onClose,
      onSubmitSuccess: onUpdated,
      mode: "edit",
      activityId: activity?.id,
    });
  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Edit Activity">
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
        />
        <ModalInput
          label="Location"
          value={form.location}
          onChange={handleChange("location")}
        />
        <ModalInput
          label="Description"
          value={form.description}
          onChange={handleChange("description")}
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
            error={errors.estimatedCost}
            onChange={handleChange("estimatedCost")}
          />
        </div>
        <div className="flex flex-col gap-1">
          <label className="text-sm font-medium text-slate-600">Status</label>
          <select
            value={form.status}
            onChange={handleChange("status") as any}
            className="w-full px-4 py-2.5 rounded-xl border border-slate-200 text-slate-800
              text-sm focus:outline-none focus:ring-2 focus:ring-blue-300 bg-white"
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
            onClick={() => handleSubmit(activity?.estimatedCost || 0)}
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
