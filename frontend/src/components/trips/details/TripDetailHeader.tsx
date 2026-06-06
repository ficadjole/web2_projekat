import { useLocation, useNavigate } from "react-router-dom";
import type { TripDetailHeaderProps } from "../../../props/TripDetailHeaderProps";
import { useState } from "react";
import { useServices } from "../../../contexts/ServiceContext";

export function TripDetailHeader({ trip, onShare }: TripDetailHeaderProps) {
  const navigate = useNavigate();
  const location = useLocation();
  const { tripShareApiService } = useServices();
  const formatDate = (dateStr: string) =>
    new Date(dateStr).toLocaleDateString("en-GB", {
      day: "numeric",
      month: "long",
      year: "numeric",
    });

  const [isGenerating, setIsGenerating] = useState(false);

  const handleDownload = async () => {
    setIsGenerating(true);
    try {
      await tripShareApiService.downloadReport(trip.id);
    } catch (err) {
      console.error("Failed to generate report.");
    } finally {
      setIsGenerating(false);
    }
  };

  return (
    <div className="bg-white rounded-2xl border border-slate-100 shadow-sm p-6 mb-6">
      <div className="flex items-start justify-between mb-4">
        <button
          onClick={() => navigate(location.state?.from || "/home")}
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
            <button
              onClick={onShare}
              className="flex items-center gap-1.5 px-4 py-2 rounded-xl
    border border-slate-200 text-slate-600 hover:bg-slate-50
    text-sm font-medium transition-colors"
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
                  d="M8.684 13.342C8.886 12.938 9 12.482 9 12c0-.482-.114-.938-.316-1.342m0 2.684a3 3 0 110-2.684m0 2.684l6.632 3.316m-6.632-6l6.632-3.316m0 0a3 3 0 105.367-2.684 3 3 0 00-5.367 2.684zm0 9.316a3 3 0 105.368 2.684 3 3 0 00-5.368-2.684z"
                />
              </svg>
              Share
            </button>
            <button
              onClick={handleDownload}
              disabled={isGenerating}
              className="flex items-center gap-1.5 px-4 py-2 rounded-xl
        border border-slate-200 text-slate-600 hover:bg-slate-50
        text-sm font-medium transition-colors disabled:opacity-50"
            >
              {isGenerating ? (
                <>
                  <svg
                    className="animate-spin w-4 h-4"
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
                  Generating...
                </>
              ) : (
                <>
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
                      d="M12 10v6m0 0l-3-3m3 3l3-3m2 8H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"
                    />
                  </svg>
                  Download PDF
                </>
              )}
            </button>
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
