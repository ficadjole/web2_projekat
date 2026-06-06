import { useEffect, useState } from "react";
import { recalculateFinances } from "../../helpers/recalculateFinances";
import type { Activity } from "../../models/tripService/Activity";
import type { Destination } from "../../models/tripService/Destination";
import type { Expense } from "../../models/tripService/Expense";
import { useServices } from "../../contexts/ServiceContext";
import { useParams } from "react-router-dom";
import type { TripDetails } from "../../models/tripService/TripDetails";

export function useTripDetails() {
  const { id } = useParams<{ id: string }>();
  const { tripApiService } = useServices();

  const [trip, setTrip] = useState<TripDetails | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");
  const [activeTab, setActiveTab] = useState<
    "overview" | "calendar" | "expenses" | "checklist"
  >("overview");

  const [isDestinationModalOpen, setIsDestinationModalOpen] = useState(false);
  const [isShareModalOpen, setIsShareModalOpen] = useState(false);

  useEffect(() => {
    if (!id) return;
    setIsLoading(true);
    tripApiService
      .getTripWithDetails(id)
      .then(setTrip)
      .catch((err) => setError(err.response?.data || "Failed to load trip."))
      .finally(() => setIsLoading(false));
  }, [id, tripApiService]);

  const handleDestinationCreated = (newDest: Destination) => {
    setTrip((prev) => {
      if (!prev) return null;
      const updatedDestinations = [
        ...(prev.destinations ?? []),
        { ...newDest, activities: [] },
      ];
      return { ...prev, destinations: updatedDestinations };
    });
  };

  const handleDestinationDeleted = (destId: string) => {
    setTrip((prev) => {
      if (!prev) return null;
      const updatedDestinations = (prev.destinations ?? []).filter(
        (d) => d.id !== destId,
      );
      return recalculateFinances({
        ...prev,
        destinations: updatedDestinations,
      });
    });
  };

  const handleDestinationUpdated = (updatedDest: Destination) => {
    setTrip((prev) => {
      if (!prev) return null;
      const updatedDestinations = (prev.destinations ?? []).map((d) =>
        d.id === updatedDest.id
          ? { ...updatedDest, activities: d.activities }
          : d,
      );
      return recalculateFinances({
        ...prev,
        destinations: updatedDestinations,
      });
    });
  };

  const handleActivityCreated = (destId: string, newActivity: Activity) => {
    setTrip((prev) => {
      if (!prev) return null;
      const updatedDestinations = (prev.destinations ?? []).map((d) =>
        d.id === destId
          ? { ...d, activities: [...(d.activities ?? []), newActivity] }
          : d,
      );
      return recalculateFinances({
        ...prev,
        destinations: updatedDestinations,
      });
    });
  };

  const handleActivityUpdated = (destId: string, updatedActivity: Activity) => {
    setTrip((prev) => {
      if (!prev) return null;
      const updatedDestinations = (prev.destinations ?? []).map((d) =>
        d.id === destId
          ? {
              ...d,
              activities: (d.activities ?? []).map((a) =>
                a.id === updatedActivity.id ? updatedActivity : a,
              ),
            }
          : d,
      );
      return recalculateFinances({
        ...prev,
        destinations: updatedDestinations,
      });
    });
  };

  const handleActivityDeleted = (destId: string, activityId: string) => {
    setTrip((prev) => {
      if (!prev) return null;
      const updatedDestinations = (prev.destinations ?? []).map((d) =>
        d.id === destId
          ? {
              ...d,
              activities: (d.activities ?? []).filter(
                (a) => a.id !== activityId,
              ),
            }
          : d,
      );
      return recalculateFinances({
        ...prev,
        destinations: updatedDestinations,
      });
    });
  };

  const handleExpenseAdded = (newExpense: Expense) => {
    setTrip((prev) => {
      if (!prev) return null;
      const updatedExpenses = [...(prev.expenses ?? []), newExpense];
      return recalculateFinances({ ...prev, expenses: updatedExpenses });
    });
  };

  const handleExpenseDeleted = (expenseId: string) => {
    setTrip((prev) => {
      if (!prev) return null;
      const updatedExpenses = (prev.expenses ?? []).filter(
        (e) => e.id !== expenseId,
      );
      return recalculateFinances({ ...prev, expenses: updatedExpenses });
    });
  };

  return {
    id,
    trip,
    isLoading,
    error,
    activeTab,
    setActiveTab,
    isDestinationModalOpen,
    setIsDestinationModalOpen,
    isShareModalOpen,
    setIsShareModalOpen,
    handleDestinationCreated,
    handleDestinationDeleted,
    handleDestinationUpdated,
    handleActivityCreated,
    handleActivityUpdated,
    handleActivityDeleted,
    handleExpenseAdded,
    handleExpenseDeleted,
  };
}
