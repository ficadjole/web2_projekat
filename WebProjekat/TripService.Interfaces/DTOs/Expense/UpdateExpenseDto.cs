
using System.Runtime.Serialization;
using TripService.Enums;

namespace TripService.Interfaces.DTOs.Expense
{
    [DataContract]
    public class UpdateExpenseDto
    {
        [DataMember]
        public string Name { get; set; } = string.Empty;
        [DataMember]
        public BudgetCategory Category { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string Description { get; set; } = string.Empty;
    }
}
