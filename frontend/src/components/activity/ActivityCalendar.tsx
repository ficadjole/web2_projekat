import { useMemo } from "react";
import type { ActivityCalendarProps } from "../../props/ActivityCalendarProps";

const statusColors: Record<string, string> = {
  Planned: "bg-blue-100 text-blue-700 border-blue-200",
  Reserved: "bg-amber-100 text-amber-700 border-amber-200",
  Finished: "bg-green-100 text-green-700 border-green-200",
  Cancelled: "bg-red-100 text-red-700 border-red-200 line-through opacity-60",
};

export function ActivityCalendar({
  destinations,
  startDate,
  endDate,
}: ActivityCalendarProps) {
  const days = useMemo(() => {
    const result = [];
    const start = new Date(startDate);
    const end = new Date(endDate);
    const current = new Date(start);

    while (current <= end) {
      const dateStr = current.toISOString().split("T")[0];
      const activities = destinations.flatMap((d) =>
        (d.activities ?? [])
          .filter((a) => a.date.split("T")[0] === dateStr)
          .map((a) => ({ ...a, destinationName: d.name })),
      );
      result.push({ date: new Date(current), dateStr, activities });
      current.setDate(current.getDate() + 1);
    }
    return result;
  }, [destinations, startDate, endDate]);

  const formatDay = (date: Date) =>
    date.toLocaleDateString("en-GB", {
      weekday: "short",
      day: "numeric",
      month: "short",
    });

  return (
    <div className="bg-white rounded-2xl border border-slate-100 shadow-sm p-5">
      <h3 className="font-semibold text-slate-800 mb-4">📅 Calendar View</h3>
      <div className="flex flex-col gap-3">
        {days.map((day) => (
          <div key={day.dateStr} className="flex gap-4">
            <div className="w-24 flex-shrink-0 pt-0.5">
              <p className="text-xs font-semibold text-slate-500">
                {formatDay(day.date)}
              </p>
            </div>

            <div className="flex-1 min-h-[2rem]">
              {day.activities.length === 0 ? (
                <div
                  className="h-8 border-l-2 border-dashed border-slate-100 pl-3
                  flex items-center"
                >
                  <span className="text-xs text-slate-300">No activities</span>
                </div>
              ) : (
                <div className="border-l-2 border-blue-200 pl-3 flex flex-col gap-1.5">
                  {day.activities.map((activity) => (
                    <div
                      key={activity.id}
                      className={`px-2.5 py-1.5 rounded-lg border text-xs font-medium
                        ${statusColors[activity.status]}`}
                    >
                      <span className="font-semibold">{activity.name}</span>
                      <span className="text-opacity-70 ml-1.5">
                        · {activity.destinationName}
                      </span>
                      {activity.estimatedCost > 0 && (
                        <span className="ml-1.5 opacity-70">
                          ${activity.estimatedCost}
                        </span>
                      )}
                    </div>
                  ))}
                </div>
              )}
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
