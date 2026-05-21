using Common.Enums;
using TripService.Enums;
using WebProjekat.Common;

namespace TripService.Models
{
    public class Expense
    {

        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public BudgetCategory Category { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Description { get; set; } = string.Empty;

        public Guid TripId { get; set; }

        public Trip Trip { get; set; }

        private Expense(Guid id, string name, BudgetCategory category, decimal amount, DateTime createdAt, string description, Guid tripId)
        {
            Id = id;
            Name = name;
            Category = category;
            Amount = amount;
            CreatedAt = createdAt;
            Description = description;
            TripId = tripId;
        }

        protected Expense() { }

        public static Result<Expense> Create(string name, BudgetCategory category, decimal amount, DateTime createdAt, string description, string tripId)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<Expense>.Failure("Name is required.", ErrorType.Validation);

            if (amount < 0)
                return Result<Expense>.Failure("Amount cannot be negative", ErrorType.Validation);

            if (string.IsNullOrWhiteSpace(description))
                return Result<Expense>.Failure("Description is requried", ErrorType.Validation);

            if (Guid.TryParse(tripId, out var TripId) == false)
                return Result<Expense>.Failure("Invalid trip ID.", ErrorType.Validation);

            var expense = new Expense(new Guid(), name, category, amount, createdAt, description, TripId);

            return Result<Expense>.Success(expense);
        }

        public static Result<Expense> Load(string id, string name, BudgetCategory category, decimal amount, DateTime createdAt, string description, string tripId)
        {
            if (Guid.TryParse(id, out var guid) == false)
                return Result<Expense>.Failure("Invalid expense ID.", ErrorType.Validation);

            if (Guid.TryParse(tripId, out var TripId) == false)
                return Result<Expense>.Failure("Invalid trip ID.", ErrorType.Validation);

            var expense = new Expense(guid, name, category, amount, createdAt, description, TripId);

            return Result<Expense>.Success(expense);
        }

    }
}
