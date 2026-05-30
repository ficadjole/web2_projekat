import { useState } from "react";
import { useNavigate } from "react-router-dom";
import type { Destination } from "../../../models/tripService/Destination";

interface DestinationCardProps {
  destination: Destination;
  tripId: string;
}

export function DestinationCard({ destination, tripId }: DestinationCardProps) {
  const navigate = useNavigate();
  const [expanded, setExpanded] = useState(false);

  const formatDate = (dateStr: string) =>
    new Date(dateStr).toLocaleDateString("en-GB", {
      day: "numeric",
      month: "short",
    });

  const statusColors = {
    Planned: "bg-blue-50 text-blue-600 border-blue-100",
    Reserved: "bg-amber-50 text-amber-600 border-amber-100",
    Finished: "bg-green-50 text-green-600 border-green-100",
    Cancelled: "bg-red-50 text-red-600 border-red-100",
  };

  return (
    <div className="bg-white rounded-2xl border border-slate-100 shadow-sm overflow-hidden">
      <div
        className="p-5 cursor-pointer hover:bg-slate-50 transition-colors"
        onClick={() => setExpanded(!expanded)}
      >
        <div className="flex items-start justify-between">
          <div className="flex items-center gap-3">
            <div
              className="w-9 h-9 rounded-xl bg-gradient-to-br from-cyan-50 to-blue-50
              border border-blue-100 flex items-center justify-center text-base"
            >
              📍
            </div>
            <div>
              <h4 className="font-semibold text-slate-800">
                {destination.name}
              </h4>
              <p className="text-xs text-slate-400">{destination.location}</p>
            </div>
          </div>

          <div className="flex items-center gap-2">
            <span className="text-xs text-slate-400">
              {formatDate(destination.arrivingDate)} —{" "}
              {formatDate(destination.leavingDate)}
            </span>
            <svg
              className={`w-4 h-4 text-slate-400 transition-transform ${expanded ? "rotate-180" : ""}`}
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M19 9l-7 7-7-7"
              />
            </svg>
          </div>
        </div>

        {destination.description && (
          <p className="text-sm text-slate-500 mt-2 ml-12 line-clamp-2">
            {destination.description}
          </p>
        )}
      </div>

      {expanded && (
        <div className="border-t border-slate-100 px-5 pb-5">
          <div className="flex items-center justify-between py-3 mb-2">
            <span className="text-xs font-semibold text-slate-500 uppercase tracking-wide">
              Activities ({destination.activities?.length ?? 0})
            </span>
            <button
              onClick={() =>
                navigate(
                  `/trips/${tripId}/destinations/${destination.id}/activities/create`,
                )
              }
              className="flex items-center gap-1 text-xs font-medium text-blue-500
                hover:text-blue-700 transition-colors"
            >
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
                  d="M12 4v16m8-8H4"
                />
              </svg>
              Add Activity
            </button>
          </div>

          {destination.activities?.length === 0 ? (
            <p className="text-sm text-slate-400 text-center py-4">
              No activities yet
            </p>
          ) : (
            <div className="flex flex-col gap-2">
              {destination.activities?.map((activity) => (
                <div
                  key={activity.id}
                  className="flex items-center justify-between px-3 py-2.5
                    bg-slate-50 rounded-xl border border-slate-100"
                >
                  <div>
                    <p className="text-sm font-medium text-slate-700">
                      {activity.name}
                    </p>
                    <p className="text-xs text-slate-400">
                      {activity.location}
                    </p>
                  </div>
                  <div className="flex items-center gap-2">
                    <span
                      className={`text-xs px-2 py-0.5 rounded-full border font-medium
                      ${statusColors[activity.status]}`}
                    >
                      {activity.status}
                    </span>
                    {activity.estimatedCost > 0 && (
                      <span className="text-xs font-medium text-slate-600">
                        ${activity.estimatedCost}
                      </span>
                    )}
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      )}
    </div>
  );
}
