using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProjekat.Common;

namespace CheckListService.Models
{
    public class Checklist
    {
        public Guid Id { get; set; }

        public Guid TripId { get; set; }

        public Guid UserId { get; set; }

        public ICollection<ChecklistItem> Items {  get; set; } = new List<ChecklistItem>();

        protected Checklist() { }

        private Checklist(Guid id, Guid tripId, Guid userId)
        {
            Id = id;
            TripId = tripId;
            UserId = userId;
        }

        public static Result<Checklist> Create(string tripId, string userId) {

            if (!Guid.TryParse(tripId, out Guid tripid))
                return Result<Checklist>.Failure("Invalid trip id", ErrorType.Validation);

            if(!Guid.TryParse(userId, out Guid userid))
                return Result<Checklist>.Failure("Invalid user id",ErrorType.Validation);

            var list = new Checklist(new Guid(), tripid, userid);

            return Result<Checklist>.Success(list); 

        }

        public static Result<Checklist> Load(string id, Guid tripId, Guid userId) {

            if (!Guid.TryParse(id, out Guid Id))
                return Result<Checklist>.Failure("Invalid checklist id", ErrorType.Validation);

            var list = new Checklist(Id, tripId, userId);

            return Result<Checklist>.Success(list);
        }
    }
}
