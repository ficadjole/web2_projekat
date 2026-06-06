import { useCallback, useEffect, useState } from "react";
import { useServices } from "../../contexts/ServiceContext";
import type { Trip } from "../../models/tripService/Trip";

export function useAdminTrips() {
  const [trips, setTrips] = useState<Trip[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");
  const { tripApiService } = useServices();

  const fetchTrips = useCallback(async () => {
    setIsLoading(true);
    setError("");
    try {
      const data = await tripApiService.getAllTripsAdmin();
      setTrips(data);
    } catch (err: any) {
      setError(err.response?.data || "Neuspešno učitavanje putovanja.");
    } finally {
      setIsLoading(false);
    }
  }, [tripApiService]);

  useEffect(() => {
    fetchTrips();
  }, [fetchTrips]);

  const handleDeleteTrip = useCallback((id: string) => {
    setTrips((prevTrips) => prevTrips.filter((trip) => trip.id !== id));
  }, []);

  const handleUpdateTrip = useCallback((updatedTrip: Trip) => {
    setTrips((prevTrips) =>
      prevTrips.map((trip) =>
        trip.id === updatedTrip.id ? updatedTrip : trip,
      ),
    );
  }, []);

  return {
    trips,
    isLoading,
    error,
    refreshTrips: fetchTrips,
    handleDeleteTrip,
    handleUpdateTrip,
  };
}
