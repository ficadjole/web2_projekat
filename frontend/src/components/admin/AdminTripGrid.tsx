import type { AdminTripGridProps } from "../../props/AdminTripGridProps";
import { TripCard } from "../trips/TripCard";

export function AdminTripGrid({
  trips,
  onUpdated,
  onDeleted,
}: AdminTripGridProps) {
  if (trips.length === 0) {
    return (
      <div className="flex flex-col items-center justify-center py-20 gap-4">
        <span className="text-5xl">🗺️</span>
        <div className="text-center">
          <p className="text-lg font-semibold text-slate-700">
            Nema kreiranih putovanja
          </p>
          <p className="text-sm text-slate-400 mt-1">
            Trenutno ne postoji nijedno putovanje u bazi podataka.
          </p>
        </div>
      </div>
    );
  }

  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
      {trips.map((trip) => (
        <TripCard
          key={trip.id}
          trip={trip}
          onUpdated={onUpdated}
          onDeleted={onDeleted}
        />
      ))}
    </div>
  );
}
