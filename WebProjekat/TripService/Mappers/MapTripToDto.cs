using TripService.Interfaces.DTOs.Trip;
using TripService.Models;

namespace TripService.Mappers
{
    public static class MapTripToDto
    {
        public static TripDto MapToDto(Trip trip) => new()
        {
            Id = trip.Id,
            Name = trip.Name,
            Description = trip.Description,
            StartDate = trip.StartDate,
            EndDate = trip.EndDate,
            PlannedBudget = trip.PlannedBudget,
            UserId = trip.UserId
        };
    }
}
