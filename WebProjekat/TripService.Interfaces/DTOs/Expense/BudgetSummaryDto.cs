
using System.Runtime.Serialization;

namespace TripService.Interfaces.DTOs.Expense
{
    [DataContract]
    public class BudgetSummaryDto
    {
        [DataMember]
        public decimal PlannedBudget { get; set; }
        [DataMember]
        public decimal TotalExpenses { get; set; }
        [DataMember]
        public decimal RemainingBudget { get; set; }
        [DataMember]
        public IEnumerable<ExpenseDto> Expenses { get; set; }
    }
}
