using Microsoft.AspNetCore.Mvc;

namespace WebApi.DTOs.Trip
{
    public class CreateTripRequest
    {
        [FromForm(Name = "name")]
        public string Name { get; set; } = string.Empty;

        [FromForm(Name = "description")]
        public string Description { get; set; } = string.Empty;
        [FromForm(Name = "startDate")]
        public DateTime StartDate { get; set; }
        [FromForm(Name = "endDate")]
        public DateTime EndDate { get; set; }
        [FromForm(Name = "plannedBudget")]
        public decimal PlannedBudget { get; set; }
    }
}
