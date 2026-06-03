import type { TripDetails } from "../models/tripService/TripDetails";

export const recalculateFinances = (tripData: TripDetails) => {
  const expensesTotal =
    tripData.expenses?.reduce((sum, e) => sum + e.amount, 0) || 0;
  const activitiesTotal =
    tripData.destinations?.reduce((sum, d) => {
      return (
        sum +
        (d.activities?.reduce(
          (aSum, act) => aSum + Number(act.estimatedCost || 0),
          0,
        ) || 0)
      );
    }, 0) || 0;

  const total = expensesTotal + activitiesTotal;
  return {
    ...tripData,
    totalExpenses: total,
    remainingBudget: tripData.plannedBudget - total,
  };
};
