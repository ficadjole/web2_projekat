import { useState, useEffect } from "react";
import TripList from "../components/trips/TripList";
import type { Trip } from "../models/tripService/Trip";
import { Navbar } from "../components/layout/Navbar";
import { tripApiService } from "../api_services/trip/TripApiService";
import { CreateTripModal } from "../components/trips/CreateTripModal";

export function HomePage() {
  const [trips, setTrips] = useState<Trip[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);

  useEffect(() => {
    tripApiService
      .getAllTrips()
      .then(setTrips)
      .catch((err) => setError(err.response?.data || "Failed to load trips."))
      .finally(() => setIsLoading(false));
  }, []);

  const handleTripCreated = (trip: Trip) => {
    setTrips((prev) => [...prev, trip]);
  };

  const handleTripDeleted = (deletedId: string) => {
    setTrips((prevTrips) => prevTrips.filter((t) => t.id !== deletedId));
  };

  const handleTripUpdated = (updatedTrip: Trip) => {
    setTrips((prevTrips) =>
      prevTrips.map((trip) =>
        trip.id === updatedTrip.id ? updatedTrip : trip,
      ),
    );
  };

  return (
    <div className="min-h-screen bg-slate-50">
      <Navbar />

      <main className="max-w-6xl mx-auto px-6 py-8">
        <div className="mb-6">
          <h1 className="text-2xl font-bold text-slate-800">My Trips</h1>
          <p className="text-slate-400 text-sm mt-1">
            All your travel plans in one place
          </p>
        </div>

        {error && (
          <div className="mb-4 px-4 py-3 bg-red-50 border border-red-100 rounded-xl text-red-500 text-sm">
            {error}
          </div>
        )}

        {isLoading ? (
          <div className="flex items-center justify-center py-20">
            <div className="flex flex-col items-center gap-3 text-slate-400">
              <svg
                className="animate-spin w-6 h-6"
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
              <span className="text-sm">Loading trips...</span>
            </div>
          </div>
        ) : (
          <TripList
            trips={trips}
            onAddTrip={() => setIsCreateModalOpen(true)}
            onDeleted={handleTripDeleted}
            onUpdated={handleTripUpdated}
          />
        )}
      </main>
      <CreateTripModal
        isOpen={isCreateModalOpen}
        onClose={() => setIsCreateModalOpen(false)}
        onCreated={handleTripCreated}
      />
    </div>
  );
}
