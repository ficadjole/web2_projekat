using CheckListService.Interfaces.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripService.Models;

namespace TripService.Documents.Model
{
    public class TripReportData
    {
        public Trip Trip { get; set; }
        public List<DestinationWithActivities> Destinations { get; set; }
        public List<Expense> Expenses { get; set; }
        public List<ChecklistItemDto> ChecklistItems { get; set; }
        public decimal TotalExpenses =>
        (Expenses?.Sum(e => e.Amount) ?? 0) +
        (Destinations?.SelectMany(d => d.Activities)
                      .Sum(a => a.EstimatedCost) ?? 0);
    }
    public class DestinationWithActivities : Destination
    {
        public List<Activity> Activities { get; set; } = new();
    }
}
