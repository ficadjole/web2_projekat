using Common.Enums;
using TripService.Enums;
using TripService.Interfaces;
using TripService.Interfaces.DTOs.Expense;
using TripService.Mappers;
using TripService.Models;
using WebProjekat.Common;

namespace TripService.Services
{
    public class ExpenseBusinessService : IExpenseBusinessService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly ITripRepository _tripRepository;

        public ExpenseBusinessService(IExpenseRepository expenseRepository,
                                      ITripRepository tripRepository)
        {
            _expenseRepository = expenseRepository;
            _tripRepository = tripRepository;
        }

        public async Task<Result<ExpenseDto>> CreateAsync(CreateExpenseDto dto, Guid userId)
        {
            var trip = await _tripRepository.GetByIdAsync(dto.TripId);
            if (trip is null)
                return Result<ExpenseDto>.Failure("Trip not found.", ErrorType.NotFound);

            if (trip.UserId != userId)
                return Result<ExpenseDto>.Failure("Unauthorized.", ErrorType.Unauthorized);

            if (dto.Amount <= 0)
                return Result<ExpenseDto>.Failure("Amount must be greater than zero.", ErrorType.Validation);

            var expense = Expense.Create(dto.Name, dto.Category, dto.Amount, DateTime.UtcNow, dto.Description, dto.TripId.ToString());

            if (expense.IsFailure)
                return Result<ExpenseDto>.Failure("Cannot create expense", ErrorType.Unexpected);

            await _expenseRepository.AddAsync(expense.Value);
            return Result<ExpenseDto>.Success(MapExpenseToDto.MapToDto(expense.Value));
        }

        public async Task<Result<ExpenseDto>> GetByIdAsync(Guid id, Guid userId)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense is null)
                return Result<ExpenseDto>.Failure("Expense not found.", ErrorType.NotFound);

            var trip = await _tripRepository.GetByIdAsync(expense.TripId);

            if (trip is null)
                return Result<ExpenseDto>.Failure("Trip not found", ErrorType.NotFound);

            if (trip.UserId != userId)
                return Result<ExpenseDto>.Failure("Unauthorized.", ErrorType.Unauthorized);

            return Result<ExpenseDto>.Success(MapExpenseToDto.MapToDto(expense));
        }

        public async Task<Result<IEnumerable<ExpenseDto>>> GetByTripIdAsync(Guid tripId, Guid userId)
        {
            var trip = await _tripRepository.GetByIdAsync(tripId);
            if (trip is null)
                return Result<IEnumerable<ExpenseDto>>.Failure("Trip not found.", ErrorType.NotFound);

            if (trip.UserId != userId)
                return Result<IEnumerable<ExpenseDto>>.Failure("Unauthorized.", ErrorType.Unauthorized);

            var expenses = await _expenseRepository.GetByTripIdAsync(tripId);
            return Result<IEnumerable<ExpenseDto>>.Success(expenses.Select(MapExpenseToDto.MapToDto));
        }

        public async Task<Result<IEnumerable<ExpenseDto>>> GetByCategoryAsync(Guid tripId, BudgetCategory category, Guid userId)
        {
            var trip = await _tripRepository.GetByIdAsync(tripId);
            if (trip is null)
                return Result<IEnumerable<ExpenseDto>>.Failure("Trip not found.", ErrorType.NotFound);

            if (trip.UserId != userId)
                return Result<IEnumerable<ExpenseDto>>.Failure("Unauthorized.", ErrorType.Unauthorized);

            var expenses = await _expenseRepository.GetByCategoryAsync(tripId, category);
            return Result<IEnumerable<ExpenseDto>>.Success(expenses.Select(MapExpenseToDto.MapToDto));
        }

        public async Task<Result<BudgetSummaryDto>> GetBudgetSummaryAsync(Guid tripId, Guid userId)
        {
            var trip = await _tripRepository.GetByIdAsync(tripId);
            if (trip is null)
                return Result<BudgetSummaryDto>.Failure("Trip not found.", ErrorType.NotFound);

            if (trip.UserId != userId)
                return Result<BudgetSummaryDto>.Failure("Unauthorized.", ErrorType.Unauthorized);

            var expenses = await _expenseRepository.GetByTripIdAsync(tripId);
            var total = await _expenseRepository.GetTotalByTripIdAsync(tripId);

            var summary = new BudgetSummaryDto
            {
                PlannedBudget = trip.PlannedBudget,
                TotalExpenses = total,
                RemainingBudget = trip.PlannedBudget - total,
                Expenses = expenses.Select(MapExpenseToDto.MapToDto)
            };

            return Result<BudgetSummaryDto>.Success(summary);
        }

        public async Task<Result<ExpenseDto>> UpdateAsync(Guid id, UpdateExpenseDto dto, Guid userId)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense is null)
                return Result<ExpenseDto>.Failure("Expense not found.", ErrorType.NotFound);

            var trip = await _tripRepository.GetByIdAsync(expense.TripId);

            if (trip is null)
                return Result<ExpenseDto>.Failure("Trip not found.", ErrorType.NotFound);

            if (trip.UserId != userId)
                return Result<ExpenseDto>.Failure("Unauthorized.", ErrorType.Unauthorized);

            if (dto.Amount <= 0)
                return Result<ExpenseDto>.Failure("Amount must be greater than zero.", ErrorType.Validation);

            expense.Name = dto.Name;
            expense.Category = dto.Category;
            expense.Amount = dto.Amount;
            expense.Description = dto.Description;

            await _expenseRepository.UpdateAsync(expense);
            return Result<ExpenseDto>.Success(MapExpenseToDto.MapToDto(expense));
        }

        public async Task<Result> DeleteAsync(Guid id, Guid userId)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense is null)
                return Result.Failure("Expense not found.", ErrorType.NotFound);

            var trip = await _tripRepository.GetByIdAsync(expense.TripId);

            if (trip is null)
                return Result.Failure("Trip not found", ErrorType.NotFound);

            if (trip.UserId != userId)
                return Result.Failure("Unauthorized.", ErrorType.Unauthorized);

            await _expenseRepository.DeleteAsync(id);
            return Result.Success();
        }
    }
}
