

using TripService.Interfaces.DTOs.Activity;
using TripService.Models;

namespace TripService.Mappers
{
    public static class MapActivityToDto
    {
        public static ActivityDto MapToDto(Activity a) => new()
        {
            Id = a.Id,
            Name = a.Name,
            Location = a.Location,
            Description = a.Description,
            Date = a.Date,
            EstimatedCost = a.EstimatedCost,
            Status = a.Status,
            DestinationId = a.DestinationId
        };
    }
}
