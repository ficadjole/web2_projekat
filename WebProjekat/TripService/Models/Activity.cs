
using Common.Enums;
using TripService.Enums;
using WebProjekat.Common;

namespace TripService.Models
{
    public class Activity
    {

        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.Now;

        public decimal EstimatedCost { get; set; }

        public ActivityStatus Status { get; set; } = ActivityStatus.Planned;

        public Guid DestinationId { get; set; }

        public Destination Destination { get; set; }

        private Activity(Guid id, string name, string location, string description, DateTime date, decimal estimatedCost, ActivityStatus status, Guid destinationId)
        {
            Id = id;
            Name = name;
            Location = location;
            Description = description;
            Date = date;
            EstimatedCost = estimatedCost;
            Status = status;
            DestinationId = destinationId;
        }

        protected Activity() { }

        public static Result<Activity> Create(string name, string location, string description, DateTime date, decimal estimatedCost, ActivityStatus status, string destinationId)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<Activity>.Failure("Name is required.", ErrorType.Validation);

            if (string.IsNullOrWhiteSpace(description))
                return Result<Activity>.Failure("Description is requried", ErrorType.Validation);

            if (string.IsNullOrWhiteSpace(location))
                return Result<Activity>.Failure("Location is requried", ErrorType.Validation);

            if (estimatedCost < 0)
                return Result<Activity>.Failure("EstimatedCost cannot be negative", ErrorType.Validation);

            if (date < DateTime.Now)
                return Result<Activity>.Failure($"Activity date cannot be lower than {DateTime.Now}", ErrorType.Validation);

            if (Guid.TryParse(destinationId, out var DestinationId) == false)
                return Result<Activity>.Failure("Invalid destination ID.", ErrorType.Validation);

            var activity = new Activity(new Guid(), name, location, description, date, estimatedCost, status, DestinationId);

            return Result<Activity>.Success(activity);
        }

        public static Result<Activity> Load(string id, string name, string location, string description, DateTime date, decimal estimatedCost, ActivityStatus status, string destinationId)
        {
            if (Guid.TryParse(id, out var guid) == false)
                return Result<Activity>.Failure("Invalid activity ID.", ErrorType.Validation);

            if (Guid.TryParse(destinationId, out var DestinationId) == false)
                return Result<Activity>.Failure("Invalid destination ID.", ErrorType.Validation);

            var activity = new Activity(guid, name, location, description, date, estimatedCost, status, DestinationId);

            return Result<Activity>.Success(activity);
        }
    }
}
