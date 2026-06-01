import type { AddTripCardProps } from "../../props/AddTripCardProps";

export function AddTripCard({ onClick }: AddTripCardProps) {
  return (
    <div
      onClick={onClick}
      className="bg-white rounded-2xl border-2 border-dashed border-slate-200 p-5
        hover:border-blue-300 hover:bg-blue-50/30 hover:-translate-y-0.5
        transition-all duration-200 cursor-pointer group
        flex flex-col items-center justify-center min-h-[200px] gap-3"
    >
      <div
        className="w-12 h-12 rounded-full bg-blue-50 border border-blue-100
        flex items-center justify-center group-hover:bg-blue-100 transition-colors"
      >
        <svg
          className="w-6 h-6 text-blue-400 group-hover:text-blue-500 transition-colors"
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
      </div>
      <div className="text-center">
        <p className="text-sm font-medium text-slate-600 group-hover:text-blue-600 transition-colors">
          New Trip
        </p>
        <p className="text-xs text-slate-400 mt-0.5">
          Plan your next adventure
        </p>
      </div>
    </div>
  );
}
