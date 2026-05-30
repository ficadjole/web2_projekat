import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import type { TripDetails } from "../../models/tripService/TripDetails";
import { tripApiService } from "../../api_services/trip/TripApiService";
import { Navbar } from "../../components/layout/Navbar";
import { TripDetailHeader } from "../../components/trips/details/TripDetailHeader";
import { DestinationCard } from "../../components/trips/details/DestinationCard";

export function TripDetailsPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [trip, setTrip] = useState<TripDetails | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    if (!id) return;
    tripApiService
      .getTripWithDetails(id)
      .then(setTrip)
      .catch((err) => setError(err.response?.data || "Failed to load trip."))
      .finally(() => setIsLoading(false));
  }, [id]);

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
        <TripDetailHeader trip={trip} />

        <div className="flex items-center justify-between mb-4">
          <h2 className="text-lg font-semibold text-slate-800">
            Destinations
            <span className="ml-2 text-sm font-normal text-slate-400">
              ({trip.destinations?.length ?? 0})
            </span>
          </h2>
          <button
            onClick={() => navigate(`/trips/${id}/destinations/create`)}
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
            <p className="text-sm text-slate-400 mt-1">
              Add your first destination to start planning
            </p>
          </div>
        ) : (
          <div className="flex flex-col gap-3">
            {trip.destinations?.map((destination) => (
              <DestinationCard
                key={destination.id}
                destination={destination}
                tripId={id!}
              />
            ))}
          </div>
        )}
      </main>
    </div>
  );
}
