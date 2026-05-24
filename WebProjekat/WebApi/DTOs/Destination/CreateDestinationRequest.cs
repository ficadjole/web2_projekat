using Microsoft.AspNetCore.Mvc;

namespace WebApi.DTOs.Destination
{
    public class CreateDestinationRequest
    {
        [FromForm(Name = "name")]
        public string Name { get; set; } = string.Empty;
        [FromForm(Name = "description")]
        public string Description { get; set; } = string.Empty;
        [FromForm(Name = "notes")]
        public string Notes { get; set; } = string.Empty;
        [FromForm(Name = "location")]
        public string Location { get; set; } = string.Empty;
        [FromForm(Name = "arrivingDate")]
        public DateTime ArrivingDate { get; set; }
        [FromForm(Name = "leavingDate")]
        public DateTime LeavingDate { get; set; }
        [FromForm(Name = "tripId")]
        public Guid TripId { get; set; }
    }
}
