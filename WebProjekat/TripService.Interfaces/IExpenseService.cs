using Microsoft.ServiceFabric.Services.Remoting;
using TripService.Enums;
using TripService.Interfaces.DTOs.Expense;
using WebProjekat.Common;

namespace TripService.Interfaces
{
    public interface IExpenseService : IService
    {
        Task<Result<ExpenseDto>> CreateExpenseAsync(CreateExpenseDto dto, Guid userId);
        Task<Result<ExpenseDto>> GetExpenseByIdAsync(Guid id, Guid userId);
        Task<Result<IEnumerable<ExpenseDto>>> GetExpensesByTripIdAsync(Guid tripId, Guid userId);
        Task<Result<IEnumerable<ExpenseDto>>> GetExpensesByCategoryAsync(Guid tripId, BudgetCategory category, Guid userId);
        Task<Result<BudgetSummaryDto>> GetBudgetSummaryAsync(Guid tripId, Guid userId);
        Task<Result<ExpenseDto>> UpdateExpenseAsync(Guid id, UpdateExpenseDto dto, Guid userId);
        Task<Result> DeleteExpenseAsync(Guid id, Guid userId);
    }
}
