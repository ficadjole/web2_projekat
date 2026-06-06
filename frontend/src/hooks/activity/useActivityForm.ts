import { useEffect, useState } from "react";
import type { FormState } from "../../props/hookProps/ActivityHook/FormStateActivity";
import { useServices } from "../../contexts/ServiceContext";
import type { UseActivityFormProps } from "../../props/hookProps/ActivityHook/UseActivityFormProps";

export function useActivityForm({
  initialState,
  remainingBudget,
  destStartDate,
  destEndDate,
  onSubmitSuccess,
  onClose,
  mode,
  destinationId,
  activityId,
}: UseActivityFormProps) {
  const { activityApiService } = useServices();

  const [form, setForm] = useState<FormState>(initialState);
  const [errors, setErrors] = useState<Record<string, string>>({});
  const [apiError, setApiError] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    setForm(initialState);
    setErrors({});
    setApiError("");
  }, [activityId]);

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

  const validate = (oldCost = 0): boolean => {
    const newErrors: Record<string, string> = {};
    const activityDateOnly = form.date.split("T")[0];
    const destStartOnly = destStartDate.split("T")[0];
    const destEndOnly = destEndDate.split("T")[0];

    const newCost = Number(form.estimatedCost) || 0;
    const costDifference = newCost - oldCost;

    if (!form.name.trim()) newErrors.name = "Name is required.";

    if (!form.location.trim()) newErrors.location = "Location is required.";

    if (!form.date) newErrors.date = "Date is required.";

    if (form.estimatedCost && Number(form.estimatedCost) < 0) {
      newErrors.estimatedCost = "Cost cannot be negative.";
    }

    if (
      form.date &&
      (activityDateOnly < destStartOnly || activityDateOnly > destEndOnly)
    ) {
      newErrors.date = "Activity date must be within destination dates.";
    }

    if (costDifference > remainingBudget) {
      newErrors.estimatedCost = `Cost exceeds remaining budget. Max extra allowed: $${remainingBudget}.`;
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (oldCost = 0) => {
    if (!validate(oldCost)) return;

    setIsLoading(true);
    setApiError("");

    try {
      let result;

      if (mode === "create") {
        result = await activityApiService.createActivity({
          ...form,
          estimatedCost: Number(form.estimatedCost) || 0,
          destinationId: destinationId!,
        });
      } else {
        result = await activityApiService.updateActivity(
          {
            ...form,
            estimatedCost: form.estimatedCost,
          },
          activityId!,
        );
      }

      onSubmitSuccess(result);
      onClose();
    } catch (err: any) {
      setApiError(err.response?.data || "Something went wrong.");
    } finally {
      setIsLoading(false);
    }
  };

  return {
    form,
    errors,
    apiError,
    isLoading,
    handleChange,
    handleSubmit,
  };
}
