import { useEffect, useState } from "react";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import type { SharedTripResponse } from "../../dtos/SharedTripResponse";
import { tripShareApiService } from "../../api_services/tripShare/TripShareApiService";
import { DestinationCard } from "../../components/trips/details/DestinationCard";
import { useAuth } from "../../hooks/auth/useAuthHook";
import { CreateDestinationModal } from "../../components/destinations/CreateDestinationModal";
import type { Destination } from "../../models/tripService/Destination";
import type { Activity } from "../../models/tripService/Activity";

export function SharedTripPage() {
  const { token } = useParams<{ token: string }>();
  const [data, setData] = useState<SharedTripResponse | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isDestinationModalOpen, setIsDestinationModalOpen] = useState(false);
  const [error, setError] = useState("");
  const { user } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    if (!token) return;

    tripShareApiService
      .getSharedTrip(token)
      .then(setData)
      .catch((err) =>
        setError(err.response?.data || "Invalid or expired link."),
      )
      .finally(() => setIsLoading(false));
  }, [token]);

  useEffect(() => {
    if (data && data.accessType === "Edit" && !user) {
      navigate("/login", { state: { from: location.pathname } });
    }
  }, [data, user, navigate, location.pathname]);

  const handleDestinationCreated = (destination: Destination) => {
    setData((prevData) => {
      if (!prevData) return prevData;

      return {
        ...prevData,
        trip: {
          ...prevData.trip,
          destinations: [
            ...(prevData.trip.destinations ?? []),
            { ...destination, activities: [] },
          ],
        },
      };
    });
  };

  const handleDestinationUpdated = (updatedDest: Destination) => {
    if (!data) return;
    setData({
      ...data,
      trip: {
        ...data.trip,
        destinations: data.trip.destinations.map((d) =>
          d.id === updatedDest.id ? updatedDest : d,
        ),
      },
    });
  };

  const handleDestinationDeleted = (id: string) => {
    if (!data) return;
    setData({
      ...data,
      trip: {
        ...data.trip,
        destinations: data.trip.destinations.filter((d) => d.id !== id),
      },
    });
  };

  const handleActivityUpdated = (destId: string, updatedActivity: Activity) => {
    if (!data) return;
    setData({
      ...data,
      trip: {
        ...data.trip,
        destinations: data.trip.destinations.map((d) => {
          if (d.id !== destId) return d;
          return {
            ...d,
            activities: d.activities.map((a) =>
              a.id === updatedActivity.id ? updatedActivity : a,
            ),
          };
        }),
      },
    });
  };

  const handleActivityCreated = (destId: string, newActivity: Activity) => {
    if (!data) return;

    setData({
      ...data,
      trip: {
        ...data.trip,
        destinations: data.trip.destinations.map((d) => {
          if (d.id !== destId) return d;
          return {
            ...d,
            activities: [...(d.activities || []), newActivity],
          };
        }),
      },
    });
  };

  const handleActivityDeleted = (destId: string, activityId: string) => {
    if (!data) return;
    setData({
      ...data,
      trip: {
        ...data.trip,
        destinations: data.trip.destinations.map((d) => {
          if (d.id !== destId) return d;
          return {
            ...d,
            activities: d.activities.filter((a) => a.id !== activityId),
          };
        }),
      },
    });
  };

  if (isLoading)
    return (
      <div className="min-h-screen bg-slate-50 flex items-center justify-center">
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
    );

  if (error || !data)
    return (
      <div className="min-h-screen bg-slate-50 flex items-center justify-center">
        <div className="text-center">
          <span className="text-5xl">🔗</span>
          <p className="mt-3 font-semibold text-slate-700">
            {error || "Link not found"}
          </p>
        </div>
      </div>
    );

  const { trip, accessType } = data;

  return (
    <div className="min-h-screen bg-slate-50">
      <div className="bg-white border-b border-slate-100 px-6 py-4">
        <div className="max-w-4xl mx-auto flex items-center justify-between">
          <div className="flex items-center gap-2">
            <span className="text-xl">🌍</span>
            <span className="font-bold text-slate-800">TripPlanner</span>
          </div>
          <span
            className={`text-xs px-3 py-1.5 rounded-full font-medium
            ${
              accessType === "View"
                ? "bg-blue-50 text-blue-600"
                : "bg-green-50 text-green-600"
            }`}
          >
            {accessType === "View" ? "👁️ View Only" : "✏️ Edit Access"}
          </span>
        </div>
      </div>

      <main className="max-w-4xl mx-auto px-6 py-8">
        <div className="bg-white rounded-2xl border border-slate-100 shadow-sm p-6 mb-6">
          <div className="flex items-center gap-3 mb-3">
            <span className="text-2xl">✈️</span>
            <h1 className="text-2xl font-bold text-slate-800">{trip.name}</h1>
          </div>
          <p className="text-slate-500 ml-11">{trip.description}</p>
          <div className="flex items-center gap-4 mt-3 ml-11">
            <span className="text-sm text-slate-400">
              {new Date(trip.startDate).toLocaleDateString()} —{" "}
              {new Date(trip.endDate).toLocaleDateString()}
            </span>
            <span className="text-sm font-medium text-slate-600">
              Budget: ${trip.plannedBudget.toLocaleString()}
            </span>
          </div>
        </div>
        <div className="flex items-center justify-between mb-4">
          <h2 className="text-lg font-semibold text-slate-800">
            Destinations
            <span className="ml-2 text-sm font-normal text-slate-400">
              ({trip.destinations?.length ?? 0})
            </span>
          </h2>
          {data.accessType === "Edit" && (
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
          )}
        </div>

        <div className="flex flex-col gap-3">
          {trip.destinations?.map((destination) => (
            <DestinationCard
              key={destination.id}
              destination={destination}
              tripId={trip.id}
              tripStartDate={trip.startDate}
              tripEndDate={trip.endDate}
              remainingBudget={trip.remainingBudget}
              readonly={accessType === "View"}
              onActivityCreated={handleActivityCreated}
              onActivityUpdated={handleActivityUpdated}
              onActivityDeleted={handleActivityDeleted}
              onDestinationDeleted={handleDestinationDeleted}
              onDestinationUpdated={handleDestinationUpdated}
            />
          ))}
        </div>
      </main>
      <CreateDestinationModal
        isOpen={isDestinationModalOpen}
        onClose={() => setIsDestinationModalOpen(false)}
        tripId={data.trip.id}
        onCreated={handleDestinationCreated}
        tripStartDate={trip.startDate}
        tripEndDate={trip.endDate}
      />
    </div>
  );
}
