import type { FormState } from "./ActivityHook/FormStateActivity";

export interface UseActivityFormProps {
  initialState: FormState;
  remainingBudget: number;
  destStartDate: string;
  destEndDate: string;
  onSubmitSuccess: (activity: any) => void;
  onClose: () => void;
  mode: "create" | "edit";
  destinationId?: string;
  activityId?: string;
}
