using Common.Interfaces;
using TripService.Enums;
using TripService.Models;

namespace TripService.Interfaces
{
    public interface IExpenseRepository : IRepository<Expense>
    {
        Task<IEnumerable<Expense>> GetByTripIdAsync(Guid tripId);
        Task<IEnumerable<Expense>> GetByCategoryAsync(Guid tripId, BudgetCategory category);
        Task<decimal> GetTotalByTripIdAsync(Guid tripId);
    }
}
