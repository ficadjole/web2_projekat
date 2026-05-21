
using System.Runtime.Serialization;
using TripService.Interfaces.DTOs.Destination;
using TripService.Interfaces.DTOs.Expense;

namespace TripService.Interfaces.DTOs.Trip
{
    [DataContract]
    public class TripDetailsDto : TripDto
    {
        [DataMember]
        public IEnumerable<DestinationDto> Destinations { get; set; }
        [DataMember]
        public IEnumerable<ExpenseDto> Expenses { get; set; }
        [DataMember]
        public decimal TotalExpenses { get; set; }
        [DataMember]
        public decimal RemainingBudget { get; set; }
    }
}
