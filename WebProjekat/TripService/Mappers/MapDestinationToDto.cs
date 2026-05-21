using TripService.Interfaces.DTOs.Activity;
using TripService.Interfaces.DTOs.Destination;
using TripService.Models;

namespace TripService.Mappers
{
    public static class MapDestinationToDto
    {
        public static DestinationDto MapToDto(Destination d) => new()
        {
            Id = d.Id,
            Name = d.Name,
            Description = d.Description,
            Notes = d.Notes,
            Location = d.Location,
            ArrivingDate = d.ArrivingDate,
            LeavingDate = d.LeavingDate,
            TripId = d.TripId,
            Activities = d.Activities?.Select(a => new ActivityDto
            {
                Id = a.Id,
                Name = a.Name,
                Location = a.Location,
                Description = a.Description,
                Date = a.Date,
                EstimatedCost = a.EstimatedCost,
                Status = a.Status,
                DestinationId = a.DestinationId
            }) ?? []
        };
    }
}
