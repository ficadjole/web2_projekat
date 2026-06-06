import type { ChecklistSectionProps } from "../../props/ChecklistSectionProps";
import { useChecklist } from "../../hooks/checklist/useChecklist";

export function ChecklistSection({ tripId }: ChecklistSectionProps) {
  const {
    items,
    newItemName,
    setNewItemName,
    isAdding,
    isLoading,
    totalCount,
    completedCount,
    completionPercentage,
    handleAddItem,
    handleToggle,
    handleDelete,
  } = useChecklist(tripId);

  return (
    <div className="bg-white rounded-2xl border border-slate-100 shadow-sm p-5">
      <div className="flex items-center justify-between mb-4">
        <div>
          <h3 className="font-semibold text-slate-800">Packing List</h3>
          {totalCount > 0 && (
            <p className="text-xs text-slate-400 mt-0.5">
              {completedCount} of {totalCount} completed
            </p>
          )}
        </div>
        {totalCount > 0 && (
          <div className="text-xs font-medium text-blue-500">
            {completionPercentage}%
          </div>
        )}
      </div>

      {totalCount > 0 && (
        <div className="w-full bg-slate-100 rounded-full h-1.5 mb-4">
          <div
            className="h-1.5 rounded-full bg-green-400 transition-all duration-300"
            style={{ width: `${(completedCount / totalCount) * 100}%` }}
          />
        </div>
      )}

      <div className="flex gap-2 mb-4">
        <input
          type="text"
          value={newItemName}
          onChange={(e) => setNewItemName(e.target.value)}
          onKeyDown={(e) => e.key === "Enter" && handleAddItem()}
          placeholder="Add item (e.g. Passport, Charger...)"
          className="flex-1 px-3 py-2 rounded-xl border border-slate-200 text-sm
            text-slate-800 placeholder-slate-300 focus:outline-none
            focus:ring-2 focus:ring-blue-300 focus:border-blue-300"
        />
        <button
          onClick={handleAddItem}
          disabled={isAdding || !newItemName.trim()}
          className="px-4 py-2 rounded-xl bg-blue-500 hover:bg-blue-600
            text-white text-sm font-medium transition-colors
            disabled:opacity-50 disabled:cursor-not-allowed"
        >
          Add
        </button>
      </div>

      {isLoading ? (
        <div className="text-center py-4">
          <svg
            className="animate-spin w-5 h-5 text-slate-300 mx-auto"
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
        </div>
      ) : items.length === 0 ? (
        <p className="text-sm text-slate-400 text-center py-4">
          No items yet — add things you need to pack!
        </p>
      ) : (
        <div className="flex flex-col gap-1">
          {items.map((item) => (
            <div
              key={item.id}
              className="flex items-center gap-3 px-3 py-2.5 rounded-xl
                hover:bg-slate-50 transition-colors group"
            >
              <button
                onClick={() => handleToggle(item)}
                className={`w-5 h-5 rounded-full border-2 flex-shrink-0 transition-all
                  flex items-center justify-center
                  ${
                    item.isChecked
                      ? "bg-green-400 border-green-400"
                      : "border-slate-300 hover:border-green-400"
                  }`}
              >
                {item.isChecked && (
                  <svg
                    className="w-3 h-3 text-white"
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth={3}
                      d="M5 13l4 4L19 7"
                    />
                  </svg>
                )}
              </button>
              <span
                className={`flex-1 text-sm transition-all
                ${item.isChecked ? "line-through text-slate-300" : "text-slate-700"}`}
              >
                {item.name}
              </span>
              <button
                onClick={() => handleDelete(item.id)}
                className="p-1 rounded-lg text-slate-300 hover:text-red-400
                  hover:bg-red-50 transition-all opacity-0 group-hover:opacity-100"
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
                    d="M6 18L18 6M6 6l12 12"
                  />
                </svg>
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
