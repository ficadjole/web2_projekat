import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import type { TripDetails } from "../../models/tripService/TripDetails";
import { tripApiService } from "../../api_services/trip/TripApiService";
import type { Destination } from "../../models/tripService/Destination";
import type { Activity } from "../../models/tripService/Activity";
import type { Expense } from "../../models/tripService/Expense";
import { Navbar } from "../../components/layout/Navbar";
import { TripDetailHeader } from "../../components/trips/details/TripDetailHeader";
import { DestinationCard } from "../../components/trips/details/DestinationCard";
import { ActivityCalendar } from "../../components/activity/ActivityCalendar";
import { ExpenseSection } from "../../components/expenses/ExpenseSection";
import { ChecklistSection } from "../../components/checklist/ChecklistSection";
import { CreateDestinationModal } from "../../components/destinations/CreateDestinationModal";
import { ShareTripModal } from "../../components/trips/share/ShareTripModal";
import { recalculateFinances } from "../../helpers/recalculateFinances";

export function TripDetailsPage() {
  const { id } = useParams<{ id: string }>();
  const [trip, setTrip] = useState<TripDetails | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");
  const [activeTab, setActiveTab] = useState<
    "overview" | "calendar" | "expenses" | "checklist"
  >("overview");
  const [isShareModalOpen, setIsShareModalOpen] = useState(false);

  const [isDestinationModalOpen, setIsDestinationModalOpen] = useState(false);

  useEffect(() => {
    if (!id) return;
    tripApiService
      .getTripWithDetails(id)
      .then(setTrip)
      .catch((err) => setError(err.response?.data || "Failed to load trip."))
      .finally(() => setIsLoading(false));
  }, [id]);

  const handleDestinationCreated = (destination: Destination) => {
    setTrip((prev) =>
      prev
        ? {
            ...prev,
            destinations: [
              ...(prev.destinations ?? []),
              { ...destination, activities: [] },
            ],
          }
        : prev,
    );
  };

  const handleDestinationUpdated = (updatedDestination: Destination) => {
    setTrip((prev) => {
      if (!prev) return prev;
      const updatedDestinations = prev.destinations.map((d) =>
        d.id === updatedDestination.id
          ? { ...updatedDestination, activities: d.activities }
          : d,
      );
      return recalculateFinances({
        ...prev,
        destinations: updatedDestinations,
      });
    });
  };

  const handleActivityUpdated = (
    destinationId: string,
    updatedActivity: Activity,
  ) => {
    setTrip((prev) => {
      if (!prev) return prev;
      const updatedDestinations = prev.destinations.map((d) =>
        d.id === destinationId
          ? {
              ...d,
              activities: d.activities.map((a) =>
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

  const handleActivityCreated = (destinationId: string, activity: Activity) => {
    setTrip((prev) => {
      if (!prev) return prev;
      return {
        ...prev,
        destinations: prev.destinations.map((d) =>
          d.id === destinationId
            ? { ...d, activities: [...(d.activities ?? []), activity] }
            : d,
        ),
      };
    });
  };

  const handleExpenseAdded = (expense: Expense) => {
    setTrip((prev) => {
      if (!prev) return prev;
      const newExpenses = [...(prev.expenses ?? []), expense];
      const totalExpenses = newExpenses.reduce((sum, e) => sum + e.amount, 0);
      return {
        ...prev,
        expenses: newExpenses,
        totalExpenses,
        remainingBudget: prev.plannedBudget - totalExpenses,
      };
    });
  };

  const handleExpenseDeleted = (expenseId: string) => {
    setTrip((prev) => {
      if (!prev) return prev;
      const newExpenses = prev.expenses.filter((e) => e.id !== expenseId);
      const totalExpenses = newExpenses.reduce((sum, e) => sum + e.amount, 0);
      return {
        ...prev,
        expenses: newExpenses,
        totalExpenses,
        remainingBudget: prev.plannedBudget - totalExpenses,
      };
    });
  };

  const handleDestinationDeleted = (destinationId: string) => {
    setTrip((prev) => {
      if (!prev) return prev;
      const updatedDestinations = prev.destinations.filter(
        (d) => d.id !== destinationId,
      );
      return recalculateFinances({
        ...prev,
        destinations: updatedDestinations,
      });
    });
  };

  const handleActivityDeleted = (destinationId: string, activityId: string) => {
    setTrip((prev) => {
      if (!prev) return prev;
      const updatedDestinations = prev.destinations.map((d) =>
        d.id === destinationId
          ? {
              ...d,
              activities: d.activities.filter((a) => a.id !== activityId),
            }
          : d,
      );
      return recalculateFinances({
        ...prev,
        destinations: updatedDestinations,
      });
    });
  };

  const tabs = [
    { key: "overview", label: "🗺️ Overview" },
    { key: "calendar", label: "📅 Calendar" },
    { key: "expenses", label: "💰 Expenses" },
    { key: "checklist", label: "✅ Checklist" },
  ] as const;

  if (isLoading)
    return (
      <div className="min-h-screen bg-slate-50">
        <Navbar />
        <div className="flex items-center justify-center py-20">
          <svg
            className="animate-spin w-6 h-6 text-slate-400"
            viewBox="0 0 24 24"
            fill="none"
          >
            <circle
              className="opacity-25"
              cx="12"
              cy="12"
              r="10"
              stroke="currentColor"
              strokeWidth="4"
            />
            <path
              className="opacity-75"
              fill="currentColor"
              d="M4 12a8 8 0 018-8v8z"
            />
          </svg>
        </div>
      </div>
    );

  if (error || !trip)
    return (
      <div className="min-h-screen bg-slate-50">
        <Navbar />
        <div className="max-w-6xl mx-auto px-6 py-8">
          <p className="text-red-500">{error || "Trip not found."}</p>
        </div>
      </div>
    );

  return (
    <div className="min-h-screen bg-slate-50">
      <Navbar />
      <main className="max-w-6xl mx-auto px-6 py-8">
        <TripDetailHeader
          trip={trip}
          onShare={() => setIsShareModalOpen(true)}
        />

        <div className="flex gap-1 bg-white border border-slate-100 rounded-2xl p-1 mb-6 shadow-sm">
          {tabs.map((tab) => (
            <button
              key={tab.key}
              onClick={() => setActiveTab(tab.key)}
              className={`flex-1 py-2 rounded-xl text-sm font-medium transition-all
                ${
                  activeTab === tab.key
                    ? "bg-blue-500 text-white shadow-sm"
                    : "text-slate-500 hover:text-slate-700 hover:bg-slate-50"
                }`}
            >
              {tab.label}
            </button>
          ))}
        </div>

        {activeTab === "overview" && (
          <>
            <div className="flex items-center justify-between mb-4">
              <h2 className="text-lg font-semibold text-slate-800">
                Destinations
                <span className="ml-2 text-sm font-normal text-slate-400">
                  ({trip.destinations?.length ?? 0})
                </span>
              </h2>
              <button
                onClick={() => setIsDestinationModalOpen(true)}
                className="flex items-center gap-1.5 px-4 py-2 rounded-xl
                  bg-blue-500 hover:bg-blue-600 text-white text-sm font-medium
                  transition-colors shadow-sm"
              >
                <svg
                  className="w-4 h-4"
                  fill="none"
                  stroke="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    strokeWidth={2}
                    d="M12 4v16m8-8H4"
                  />
                </svg>
                Add Destination
              </button>
            </div>

            {trip.destinations?.length === 0 ? (
              <div className="text-center py-12 bg-white rounded-2xl border border-slate-100">
                <span className="text-4xl">📍</span>
                <p className="mt-3 text-slate-500 font-medium">
                  No destinations yet
                </p>
              </div>
            ) : (
              <div className="flex flex-col gap-3">
                {trip.destinations?.map((destination) => (
                  <DestinationCard
                    key={destination.id}
                    destination={destination}
                    tripId={id!}
                    remainingBudget={trip.remainingBudget}
                    tripStartDate={trip.startDate}
                    tripEndDate={trip.endDate}
                    onActivityCreated={handleActivityCreated}
                    onDestinationUpdated={handleDestinationUpdated}
                    onActivityUpdated={handleActivityUpdated}
                    onActivityDeleted={handleActivityDeleted}
                    onDestinationDeleted={handleDestinationDeleted}
                    readonly={false}
                  />
                ))}
              </div>
            )}
          </>
        )}

        {activeTab === "calendar" && (
          <ActivityCalendar
            destinations={trip.destinations ?? []}
            startDate={trip.startDate}
            endDate={trip.endDate}
          />
        )}

        {activeTab === "expenses" && (
          <ExpenseSection
            tripId={id!}
            plannedBudget={trip.plannedBudget}
            expenses={trip.expenses ?? []}
            totalExpenses={trip.totalExpenses}
            remainingBudget={trip.remainingBudget}
            onExpenseAdded={handleExpenseAdded}
            onExpenseDeleted={handleExpenseDeleted}
          />
        )}

        {activeTab === "checklist" && <ChecklistSection tripId={id!} />}
      </main>

      <CreateDestinationModal
        isOpen={isDestinationModalOpen}
        onClose={() => setIsDestinationModalOpen(false)}
        tripId={id!}
        onCreated={handleDestinationCreated}
        tripStartDate={trip.startDate}
        tripEndDate={trip.endDate}
      />

      <ShareTripModal
        isOpen={isShareModalOpen}
        onClose={() => setIsShareModalOpen(false)}
        tripId={id!}
      />
    </div>
  );
}
