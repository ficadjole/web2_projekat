using System.Runtime.Serialization;
using TripService.Enums;

namespace TripService.Interfaces.DTOs.Expense
{
    [DataContract]
    public class ExpenseDto
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; } = string.Empty;
        [DataMember]
        public BudgetCategory Category { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string Description { get; set; } = string.Empty;
        [DataMember]
        public DateTime CreatedAt { get; set; }
        [DataMember]
        public Guid TripId { get; set; }
    }
}
