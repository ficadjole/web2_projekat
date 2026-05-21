
using TripService.Interfaces.DTOs.Expense;
using TripService.Models;

namespace TripService.Mappers
{
    public static class MapExpenseToDto
    {
        public static ExpenseDto MapToDto(Expense e) => new()
        {
            Id = e.Id,
            Name = e.Name,
            Category = e.Category,
            Amount = e.Amount,
            Description = e.Description,
            CreatedAt = e.CreatedAt,
            TripId = e.TripId
        };
    }
}
