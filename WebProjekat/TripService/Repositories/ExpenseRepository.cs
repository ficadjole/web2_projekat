using Microsoft.EntityFrameworkCore;
using TripService.DatabaseContext;
using TripService.Enums;
using TripService.Interfaces;
using TripService.Models;

namespace TripService.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly TripsDbContext _context;

        public ExpenseRepository(TripsDbContext context)
        {
            _context = context;
        }

        public async Task<Expense?> GetByIdAsync(Guid id) => await _context.Expenses.FindAsync(id);

        public async Task<IEnumerable<Expense>> GetByTripIdAsync(Guid tripId) => await _context.Expenses
                                                                                .Where(e => e.TripId == tripId)
                                                                                .OrderByDescending(e => e.CreatedAt)
                                                                                .ToListAsync();

        public async Task<IEnumerable<Expense>> GetByCategoryAsync(Guid tripId, BudgetCategory category) => await _context.Expenses
                                                                                                            .Where(e => e.TripId == tripId && e.Category == category)
                                                                                                            .ToListAsync();

        public async Task<decimal> GetTotalByTripIdAsync(Guid tripId) => await _context.Expenses
                                                                        .Where(e => e.TripId == tripId)
                                                                        .SumAsync(e => e.Amount);

        public async Task AddAsync(Expense expense)
        {
            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Expense expense)
        {
            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var expense = await GetByIdAsync(id);
            if (expense is null) return;

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
        }
    }
}
