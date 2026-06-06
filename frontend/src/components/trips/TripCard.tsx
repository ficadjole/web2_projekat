import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { EditTripModal } from "./EditTripModal";
import { ConfirmDeleteModal } from "../ui/ConfirmDeleteModal";
import type { TripCardProps } from "../../props/TripCardProps";
import { useServices } from "../../contexts/ServiceContext";

export function TripCard({ trip, onUpdated, onDeleted }: TripCardProps) {
  const navigate = useNavigate();
  const { tripApiService } = useServices();
  const [isEditOpen, setIsEditOpen] = useState(false);
  const [isDeleteOpen, setIsDeleteOpen] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);

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

  const handleDelete = async () => {
    setIsDeleting(true);
    try {
      await tripApiService.deleteTrip(trip.id);
      onDeleted(trip.id);
    } finally {
      setIsDeleting(false);
      setIsDeleteOpen(false);
    }
  };
  return (
    <>
      <div
        className="bg-white rounded-2xl border border-slate-100 shadow-sm p-5
        hover:shadow-md hover:border-blue-100 hover:-translate-y-0.5
        transition-all duration-200 group relative"
      >
        <div className="absolute top-12 right-4 flex gap-1 opacity-0 group-hover:opacity-100 transition-opacity">
          <button
            onClick={(e) => {
              e.stopPropagation();
              setIsEditOpen(true);
            }}
            className="p-1.5 rounded-lg text-slate-400 hover:text-blue-500
              hover:bg-blue-50 transition-all"
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
                d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z"
              />
            </svg>
          </button>
          <button
            onClick={(e) => {
              e.stopPropagation();
              setIsDeleteOpen(true);
            }}
            className="p-1.5 rounded-lg text-slate-400 hover:text-red-500
              hover:bg-red-50 transition-all"
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
                d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"
              />
            </svg>
          </button>
        </div>

        <div
          onClick={() => navigate(`/trips/${trip.id}`)}
          className="cursor-pointer"
        >
          <div className="flex items-start gap-3 mb-3">
            <div
              className="w-10 h-10 rounded-xl bg-gradient-to-br from-blue-50 to-cyan-50
              border border-blue-100 flex items-center justify-center text-lg flex-shrink-0"
            >
              ✈️
            </div>
            <span
              className="text-xs font-medium text-blue-500 bg-blue-50 px-2.5 py-1
              rounded-full ml-auto"
            >
              {getDaysCount()} days
            </span>
          </div>
          <h3 className="font-semibold text-slate-800 mb-1 line-clamp-1">
            {trip.name}
          </h3>
          <p className="text-sm text-slate-400 line-clamp-2 mb-4">
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
      </div>

      <EditTripModal
        isOpen={isEditOpen}
        onClose={() => setIsEditOpen(false)}
        trip={trip}
        onUpdated={onUpdated}
      />

      <ConfirmDeleteModal
        isOpen={isDeleteOpen}
        onClose={() => setIsDeleteOpen(false)}
        onConfirm={handleDelete}
        isLoading={isDeleting}
        title="Delete Trip"
        message={`Are you sure you want to delete "${trip.name}"? This will also delete all destinations, activities and expenses.`}
      />
    </>
  );
}
