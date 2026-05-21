using TripService.Enums;
using TripService.Interfaces.DTOs.Expense;
using WebProjekat.Common;

namespace TripService.Interfaces
{
    public interface IExpenseBusinessService
    {
        Task<Result<ExpenseDto>> CreateAsync(CreateExpenseDto dto, Guid userId);
        Task<Result<ExpenseDto>> GetByIdAsync(Guid id, Guid userId);
        Task<Result<IEnumerable<ExpenseDto>>> GetByTripIdAsync(Guid tripId, Guid userId);
        Task<Result<IEnumerable<ExpenseDto>>> GetByCategoryAsync(Guid tripId, BudgetCategory category, Guid userId);
        Task<Result<BudgetSummaryDto>> GetBudgetSummaryAsync(Guid tripId, Guid userId);
        Task<Result<ExpenseDto>> UpdateAsync(Guid id, UpdateExpenseDto dto, Guid userId);
        Task<Result> DeleteAsync(Guid id, Guid userId);
    }
}
