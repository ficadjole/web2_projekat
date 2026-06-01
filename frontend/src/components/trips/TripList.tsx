import type { TripListProps } from "../../props/TripListProps";
import { AddTripCard } from "./AddTripCard";
import { TripCard } from "./TripCard";

export function TripList({
  trips,
  onAddTrip,
  onUpdated,
  onDeleted,
}: TripListProps) {
  if (trips.length === 0) {
    return (
      <div className="flex flex-col items-center justify-center py-20 gap-4">
        <span className="text-5xl">🗺️</span>
        <div className="text-center">
          <p className="text-lg font-semibold text-slate-700">No trips yet</p>
          <p className="text-sm text-slate-400 mt-1">
            Start planning your first adventure
          </p>
        </div>
        <AddTripCard onClick={onAddTrip} />
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
      <AddTripCard onClick={onAddTrip} />
    </div>
  );
}

export default TripList;
