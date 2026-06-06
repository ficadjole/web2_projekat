using Common.Enums;
using WebProjekat.Common;

namespace TripService.Models
{
    public class Trip
    {


        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(5);

        public decimal PlannedBudget { get; set; }

        public Guid UserId { get; set; }

        public string Notes {  get; set; } = string.Empty;

        public ICollection<Destination> Destinations { get; set; } = new List<Destination>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

        private Trip(string name, string description, DateTime startDate, DateTime endDate, decimal plannedBudget, Guid userId, Guid id,string notes)
        {
            Id = id;
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            PlannedBudget = plannedBudget;
            UserId = userId;
            Notes = notes;
        }

        protected Trip() { }

        public static Result<Trip> Create(string name, string description, DateTime startDate, DateTime endDate, decimal plannedBudget, Guid userId, string notes)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<Trip>.Failure("Name is required.", ErrorType.Validation);

            if (string.IsNullOrWhiteSpace(description))
                return Result<Trip>.Failure("Description is requried", ErrorType.Validation);

            if (startDate > endDate)
                return Result<Trip>.Failure("StartDate cannot be bigger than EndDate", ErrorType.Validation);

            if (plannedBudget < 0)
                return Result<Trip>.Failure("PlannedBudget cannot be negative", ErrorType.Validation);


            var trip = new Trip(name, description, startDate, endDate, plannedBudget, userId, new Guid(),notes);

            return Result<Trip>.Success(trip);

        }

        public static Result<Trip> Load(string id, string name, string description, DateTime startDate, DateTime endDate, decimal plannedBudget, Guid userId,string notes)
        {
            if (Guid.TryParse(id, out var guid) == false)
                return Result<Trip>.Failure("Invalid trip ID.", ErrorType.Validation);

            var trip = new Trip(name, description, startDate, endDate, plannedBudget, userId, guid, notes);

            return Result<Trip>.Success(trip);
        }

    }
}
