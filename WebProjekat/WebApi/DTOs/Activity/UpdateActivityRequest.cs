using Microsoft.AspNetCore.Mvc;
using TripService.Enums;

namespace WebApi.DTOs.Activity
{
    public class UpdateActivityRequest
    {
        [FromForm(Name = "name")]
        public string Name { get; set; } = string.Empty;

        [FromForm(Name = "description")]
        public string Description { get; set; } = string.Empty;

        [FromForm(Name = "location")]
        public string Location { get; set; } = string.Empty;

        [FromForm(Name = "date")]
        public DateTime Date { get; set; }

        [FromForm(Name = "estimatedCost")]
        public decimal EstimatedCost { get; set; }

        [FromForm(Name = "status")]
        public ActivityStatus Status { get; set; }
    }
}
