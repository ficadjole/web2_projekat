import { useState } from "react";
import type { Activity } from "../../../models/tripService/Activity";
import CreateActivityModal from "../../activity/CreateActivityModal";
import type { DestinationCardProps } from "../../../props/DestinationCardProps";
import EditDestinationModal from "../../destinations/EditDestinationModal";
import { EditActivityModal } from "../../activity/EditActivityModal";
import { destinationApiService } from "../../../api_services/destination/DestinationApiService";
import { activityApiService } from "../../../api_services/acitivity/ActivityApiService";
import { ConfirmDeleteModal } from "../../ui/ConfirmDeleteModal";

export function DestinationCard({
  destination,
  tripStartDate,
  tripEndDate,
  remainingBudget,
  onActivityCreated,
  onDestinationUpdated,
  onActivityUpdated,
  onDestinationDeleted,
  onActivityDeleted,
}: DestinationCardProps) {
  const [expanded, setExpanded] = useState(false);
  const [isActivityModalOpen, setIsActivityModalOpen] = useState(false);
  const [isEditDestinationOpen, setIsEditDestinationOpen] = useState(false);
  const [activityToEdit, setActivityToEdit] = useState<Activity | null>(null);

  const [deleteTarget, setDeleteTarget] = useState<{
    type: "destination" | "activity";
    id: string;
    name: string;
  } | null>(null);

  const [isDeleting, setIsDeleting] = useState(false);

  const activities = destination.activities ?? [];

  const handleActivityCreated = (activity: Activity) => {
    onActivityCreated(destination.id, activity);
  };

  const handleConfirmDelete = async () => {
    if (!deleteTarget) return;
    setIsDeleting(true);

    try {
      if (deleteTarget.type === "destination") {
        await destinationApiService.deleteDestination(deleteTarget.id);
        onDestinationDeleted(deleteTarget.id);
      } else {
        await activityApiService.deleteActivity(deleteTarget.id);
        onActivityDeleted(destination.id, deleteTarget.id);
      }
    } catch (err) {
      alert(`Failed to delete ${deleteTarget.type}.`);
    } finally {
      setIsDeleting(false);
      setDeleteTarget(null);
    }
  };

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
                <div className="flex items-center gap-1 transition-opacity">
                  <button
                    onClick={(e) => {
                      e.stopPropagation();
                      setIsEditDestinationOpen(true);
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
                      setDeleteTarget({
                        type: "destination",
                        id: destination.id,
                        name: destination.name,
                      });
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
              Activities ({activities.length})
            </span>
            <button
              onClick={() => setIsActivityModalOpen(true)}
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
                      <span className="flex items-center gap-1 ">
                        <button
                          onClick={() => setActivityToEdit(activity)}
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
                          onClick={() => {
                            setDeleteTarget({
                              type: "activity",
                              id: activity.id,
                              name: activity.name,
                            });
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
                      </span>
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
      <CreateActivityModal
        isOpen={isActivityModalOpen}
        onClose={() => setIsActivityModalOpen(false)}
        destinationId={destination.id}
        onCreated={handleActivityCreated}
        destStartDate={destination.arrivingDate}
        destEndDate={destination.leavingDate}
        remainingBudget={remainingBudget}
      />
      <EditDestinationModal
        isOpen={isEditDestinationOpen}
        onClose={() => setIsEditDestinationOpen(false)}
        destination={destination}
        onUpdated={onDestinationUpdated}
        tripStartDate={tripStartDate}
        tripEndDate={tripEndDate}
      />
      {activityToEdit && (
        <EditActivityModal
          isOpen={!!activityToEdit}
          onClose={() => setActivityToEdit(null)}
          activity={activityToEdit}
          onUpdated={(updated) => {
            onActivityUpdated(destination.id, updated);
            setActivityToEdit(null);
          }}
          remainingBudget={remainingBudget}
          destStartDate={destination.arrivingDate}
          destEndDate={destination.leavingDate}
        />
      )}
      <ConfirmDeleteModal
        isOpen={!!deleteTarget}
        onClose={() => setDeleteTarget(null)}
        onConfirm={handleConfirmDelete}
        isLoading={isDeleting}
        title={
          deleteTarget?.type === "destination"
            ? "Delete Destination"
            : "Delete Activity"
        }
        message={
          deleteTarget?.type === "destination"
            ? `Are you sure you want to delete "${deleteTarget.name}"? All activities within this destination will also be deleted.`
            : `Are you sure you want to delete activity "${deleteTarget?.name}"?`
        }
      />
    </div>
  );
}
