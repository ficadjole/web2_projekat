import { useNavigate } from "react-router-dom";
import type { Trip } from "../../models/tripService/Trip";

interface TripCardProps {
  trip: Trip;
}

export function TripCard({ trip }: TripCardProps) {
  const navigate = useNavigate();

  const formatDate = (dateStr: string) =>
    new Date(dateStr).toLocaleDateString("en-GB", {
      day: "numeric",
      month: "short",
      year: "numeric",
    });

  const getDaysCount = () => {
    const start = new Date(trip.startDate);
    const end = new Date(trip.endDate);
    return Math.ceil((end.getTime() - start.getTime()) / (1000 * 60 * 60 * 24));
  };

  return (
    <div
      onClick={() => navigate(`/trips/${trip.id}`)}
      className="bg-white rounded-2xl border border-slate-100 shadow-sm p-5
        hover:shadow-md hover:border-blue-100 hover:-translate-y-0.5
        transition-all duration-200 cursor-pointer group"
    >
      <div className="flex items-start justify-between mb-3">
        <div
          className="w-10 h-10 rounded-xl bg-gradient-to-br from-blue-50 to-cyan-50
          border border-blue-100 flex items-center justify-center text-lg flex-shrink-0"
        >
          ✈️
        </div>
        <span className="text-xs font-medium text-blue-500 bg-blue-50 px-2.5 py-1 rounded-full">
          {getDaysCount()} days
        </span>
      </div>

      <h3
        className="font-semibold text-slate-800 mb-1 group-hover:text-blue-600
        transition-colors line-clamp-1"
      >
        {trip.name}
      </h3>
      <p className="text-sm text-slate-400 line-clamp-2 mb-4 leading-relaxed">
        {trip.description}
      </p>

      <div className="flex items-center gap-1.5 text-xs text-slate-500 mb-3">
        <svg
          className="w-3.5 h-3.5"
          fill="none"
          stroke="currentColor"
          viewBox="0 0 24 24"
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            strokeWidth={2}
            d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"
          />
        </svg>
        <span>
          {formatDate(trip.startDate)} — {formatDate(trip.endDate)}
        </span>
      </div>

      <div className="flex items-center justify-between pt-3 border-t border-slate-50">
        <span className="text-xs text-slate-400">Planned budget</span>
        <span className="text-sm font-semibold text-slate-700">
          ${trip.plannedBudget.toLocaleString()}
        </span>
      </div>
    </div>
  );
}
