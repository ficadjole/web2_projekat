import { useNavigate } from "react-router-dom";
import type { TripDetailHeaderProps } from "../../../props/TripDetailHeaderProps";

export function TripDetailHeader({ trip }: TripDetailHeaderProps) {
  const navigate = useNavigate();

  const formatDate = (dateStr: string) =>
    new Date(dateStr).toLocaleDateString("en-GB", {
      day: "numeric",
      month: "long",
      year: "numeric",
    });

  return (
    <div className="bg-white rounded-2xl border border-slate-100 shadow-sm p-6 mb-6">
      <div className="flex items-start justify-between mb-4">
        <button
          onClick={() => navigate("/home")}
          className="flex items-center gap-1.5 text-sm text-slate-400
            hover:text-slate-600 transition-colors"
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
              d="M15 19l-7-7 7-7"
            />
          </svg>
          Back
        </button>
      </div>

      <div className="flex items-start justify-between">
        <div>
          <div className="flex items-center gap-2 mb-1">
            <span className="text-2xl">✈️</span>
            <h1 className="text-2xl font-bold text-slate-800">{trip.name}</h1>
          </div>
          <p className="text-slate-500 ml-9">{trip.description}</p>
          <div className="flex items-center gap-4 mt-3 ml-9">
            <span className="flex items-center gap-1.5 text-sm text-slate-400">
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
                  d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"
                />
              </svg>
              {formatDate(trip.startDate)} — {formatDate(trip.endDate)}
            </span>
          </div>
        </div>

        <div className="flex gap-3">
          <div className="text-center px-4 py-3 bg-slate-50 rounded-xl border border-slate-100">
            <p className="text-xs text-slate-400 mb-1">Budget</p>
            <p className="text-lg font-bold text-slate-700">
              ${trip.plannedBudget.toLocaleString()}
            </p>
          </div>
          <div className="text-center px-4 py-3 bg-red-50 rounded-xl border border-red-100">
            <p className="text-xs text-red-400 mb-1">Spent</p>
            <p className="text-lg font-bold text-red-600">
              ${trip.totalExpenses.toLocaleString()}
            </p>
          </div>
          <div
            className={`text-center px-4 py-3 rounded-xl border
            ${
              trip.remainingBudget >= 0
                ? "bg-green-50 border-green-100"
                : "bg-red-50 border-red-100"
            }`}
          >
            <p
              className={`text-xs mb-1 ${trip.remainingBudget >= 0 ? "text-green-400" : "text-red-400"}`}
            >
              Remaining
            </p>
            <p
              className={`text-lg font-bold
              ${trip.remainingBudget >= 0 ? "text-green-600" : "text-red-600"}`}
            >
              ${Math.abs(trip.remainingBudget).toLocaleString()}
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}
