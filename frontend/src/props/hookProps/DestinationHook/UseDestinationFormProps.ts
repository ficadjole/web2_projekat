import type { DestinationFormState } from "./DestinationFormState";

export interface UseDestinationFormProps {
  initialState: DestinationFormState;
  tripStartDate: string;
  tripEndDate: string;
  onSubmitSuccess: (destination: any) => void;
  onClose: () => void;
  mode: "create" | "edit";
  tripId?: string;
  destinationId?: string;
}
