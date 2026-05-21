using Common.Enums;
using WebProjekat.Common;

namespace TripService.Models
{
    public class Destination
    {

        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public DateTime ArrivingDate { get; set; } = DateTime.Now;

        public DateTime LeavingDate { get; set; } = DateTime.Now;

        public Guid TripId { get; set; }
        public Trip Trip { get; set; }

        public ICollection<Activity> Activities { get; set; } = new List<Activity>();

        private Destination(Guid id, string name, string description, string notes, string location, DateTime arrivingDate, DateTime leavingDate, Guid tripId)
        {
            Id = id;
            Name = name;
            Description = description;
            Notes = notes;
            Location = location;
            ArrivingDate = arrivingDate;
            LeavingDate = leavingDate;
            TripId = tripId;
        }

        protected Destination() { }

        public static Result<Destination> Create(string name, string description, string notes, string location, DateTime arrivingDate, DateTime leavingDate, string tripId)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<Destination>.Failure("Name is required.", ErrorType.Validation);

            if (string.IsNullOrWhiteSpace(description))
                return Result<Destination>.Failure("Description is requried", ErrorType.Validation);

            if (string.IsNullOrWhiteSpace(notes))
                return Result<Destination>.Failure("Notes is requried", ErrorType.Validation);

            if (string.IsNullOrWhiteSpace(location))
                return Result<Destination>.Failure("Location is requried", ErrorType.Validation);

            if (arrivingDate > leavingDate)
                return Result<Destination>.Failure("ArrivingDate cannot be bigger than LeavingDate", ErrorType.Validation);

            if (Guid.TryParse(tripId, out var TripId) == false)
                return Result<Destination>.Failure("Invalid trip ID.", ErrorType.Validation);

            var destination = new Destination(new Guid(), name, description, notes, location, arrivingDate, leavingDate, TripId);

            return Result<Destination>.Success(destination);
        }

        public static Result<Destination> Load(string id, string name, string description, string notes, string location, DateTime arrivingDate, DateTime leavingDate, string tripId)
        {
            if (Guid.TryParse(id, out var guid) == false)
                return Result<Destination>.Failure("Invalid destination ID.", ErrorType.Validation);

            if (Guid.TryParse(tripId, out var TripId) == false)
                return Result<Destination>.Failure("Invalid trip ID.", ErrorType.Validation);

            var destination = new Destination(guid, name, description, notes, location, arrivingDate, leavingDate, TripId);

            return Result<Destination>.Success(destination);
        }

    }
}