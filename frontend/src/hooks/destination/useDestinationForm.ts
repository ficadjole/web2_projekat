import { useEffect, useState } from "react";
import { useServices } from "../../contexts/ServiceContext";
import type { UseDestinationFormProps } from "../../props/hookProps/DestinationHook/UseDestinationFormProps";
import type { DestinationFormState } from "../../props/hookProps/DestinationHook/DestinationFormState";

export function useDestinationForm({
  initialState,
  tripStartDate,
  tripEndDate,
  onSubmitSuccess,
  onClose,
  mode,
  tripId,
  destinationId,
}: UseDestinationFormProps) {
  const { destinationApiService } = useServices();

  const [form, setForm] = useState<DestinationFormState>(initialState);
  const [errors, setErrors] = useState<Record<string, string>>({});
  const [apiError, setApiError] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    if (mode === "edit") {
      setForm(initialState);
    }
    setErrors({});
    setApiError("");
  }, [destinationId]);

  const handleChange =
    (field: keyof DestinationFormState) =>
    (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
      setForm((prev) => ({ ...prev, [field]: e.target.value }));
      setErrors((prev) => ({ ...prev, [field]: "" }));
    };

  const validate = (): boolean => {
    const newErrors: Record<string, string> = {};
    const formattedMin = tripStartDate ? tripStartDate.split("T")[0] : "";
    const formattedMax = tripEndDate ? tripEndDate.split("T")[0] : "";

    if (!form.name.trim()) newErrors.name = "Name is required.";
    if (!form.location.trim()) newErrors.location = "Location is required.";
    if (!form.arrivingDate)
      newErrors.arrivingDate = "Arriving date is required.";
    if (!form.leavingDate) newErrors.leavingDate = "Leaving date is required.";

    if (
      form.arrivingDate &&
      form.leavingDate &&
      form.arrivingDate > form.leavingDate
    ) {
      newErrors.leavingDate = "Leaving date cannot be before arriving date.";
    }

    if (form.arrivingDate && formattedMin && form.arrivingDate < formattedMin) {
      newErrors.arrivingDate = `Arriving date cannot be before trip start (${formattedMin}).`;
    }
    if (form.leavingDate && formattedMax && form.leavingDate > formattedMax) {
      newErrors.leavingDate = `Leaving date cannot be after trip end (${formattedMax}).`;
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async () => {
    if (!validate()) return;
    setIsLoading(true);
    setApiError("");

    try {
      let result;
      if (mode === "create") {
        result = await destinationApiService.createDestination({
          ...form,
          tripId: tripId!,
        });
      } else {
        result = await destinationApiService.updateDestination(
          {
            name: form.name,
            description: form.description,
            notes: form.notes,
            location: form.location,
            arrivingDate: form.arrivingDate,
            leavingDate: form.leavingDate,
          },
          destinationId!,
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
