import { useEffect, useState } from "react";
import { recalculateFinances } from "../../helpers/recalculateFinances";
import type { Activity } from "../../models/tripService/Activity";
import type { Destination } from "../../models/tripService/Destination";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import { useAuth } from "../auth/useAuthHook";
import { useServices } from "../../contexts/ServiceContext";
import type { SharedTripResponse } from "../../dtos/SharedTripResponse";

export function useSharedTrip() {
  const { token } = useParams<{ token: string }>();
  const { tripShareApiService } = useServices();
  const { user } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const [data, setData] = useState<SharedTripResponse | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isDestinationModalOpen, setIsDestinationModalOpen] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    if (!token) return;

    tripShareApiService
      .getSharedTrip(token)
      .then(setData)
      .catch((err) =>
        setError(err.response?.data || "Invalid or expired link."),
      )
      .finally(() => setIsLoading(false));
  }, [token, tripShareApiService]);

  useEffect(() => {
    if (data && data.accessType === "Edit" && !user) {
      navigate("/login", { state: { from: location.pathname } });
    }
  }, [data, user, navigate, location.pathname]);

  const handleDestinationCreated = (newDest: Destination) => {
    setData((prev) => {
      if (!prev) return null;
      const updatedDestinations = [
        ...(prev.trip.destinations ?? []),
        { ...newDest, activities: [] },
      ];
      return {
        ...prev,
        trip: { ...prev.trip, destinations: updatedDestinations },
      };
    });
  };

  const handleDestinationDeleted = (destId: string) => {
    setData((prev) => {
      if (!prev) return null;
      const updatedDestinations = (prev.trip.destinations ?? []).filter(
        (d) => d.id !== destId,
      );
      const updatedTrip = recalculateFinances({
        ...prev.trip,
        destinations: updatedDestinations,
      });
      return { ...prev, trip: updatedTrip };
    });
  };

  const handleDestinationUpdated = (updatedDest: Destination) => {
    setData((prev) => {
      if (!prev) return null;
      const updatedDestinations = (prev.trip.destinations ?? []).map((d) =>
        d.id === updatedDest.id
          ? { ...updatedDest, activities: d.activities }
          : d,
      );
      const updatedTrip = recalculateFinances({
        ...prev.trip,
        destinations: updatedDestinations,
      });
      return { ...prev, trip: updatedTrip };
    });
  };

  const handleActivityCreated = (destId: string, newActivity: Activity) => {
    setData((prev) => {
      if (!prev) return null;
      const updatedDestinations = (prev.trip.destinations ?? []).map((d) =>
        d.id === destId
          ? { ...d, activities: [...(d.activities ?? []), newActivity] }
          : d,
      );
      const updatedTrip = recalculateFinances({
        ...prev.trip,
        destinations: updatedDestinations,
      });
      return { ...prev, trip: updatedTrip };
    });
  };

  const handleActivityUpdated = (destId: string, updatedActivity: Activity) => {
    setData((prev) => {
      if (!prev) return null;
      const updatedDestinations = (prev.trip.destinations ?? []).map((d) =>
        d.id === destId
          ? {
              ...d,
              activities: (d.activities ?? []).map((a) =>
                a.id === updatedActivity.id ? updatedActivity : a,
              ),
            }
          : d,
      );
      const updatedTrip = recalculateFinances({
        ...prev.trip,
        destinations: updatedDestinations,
      });
      return { ...prev, trip: updatedTrip };
    });
  };

  const handleActivityDeleted = (destId: string, activityId: string) => {
    setData((prev) => {
      if (!prev) return null;
      const updatedDestinations = (prev.trip.destinations ?? []).map((d) =>
        d.id === destId
          ? {
              ...d,
              activities: (d.activities ?? []).filter(
                (a) => a.id !== activityId,
              ),
            }
          : d,
      );
      const updatedTrip = recalculateFinances({
        ...prev.trip,
        destinations: updatedDestinations,
      });
      return { ...prev, trip: updatedTrip };
    });
  };

  return {
    data,
    isLoading,
    error,
    isDestinationModalOpen,
    setIsDestinationModalOpen,
    handleDestinationCreated,
    handleDestinationDeleted,
    handleDestinationUpdated,
    handleActivityCreated,
    handleActivityUpdated,
    handleActivityDeleted,
  };
}
