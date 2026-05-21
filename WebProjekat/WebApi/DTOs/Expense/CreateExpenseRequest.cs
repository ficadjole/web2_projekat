using Microsoft.AspNetCore.Mvc;
using TripService.Enums;

namespace WebApi.DTOs.Expense
{
    public class CreateExpenseRequest
    {
        [FromForm(Name = "name")]
        public string Name { get; set; } = string.Empty;

        [FromForm(Name = "category")]
        public BudgetCategory Category { get; set; }

        [FromForm(Name = "amount")]
        public decimal Amount { get; set; }

        [FromForm(Name = "description")]
        public string Description { get; set; } = string.Empty;

        [FromForm(Name = "tripId")]
        public Guid TripId { get; set; }
    }
}
