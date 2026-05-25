using TripService.Interfaces.DTOs.Destination;
using TripService.Interfaces.DTOs.Expense;
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

        public static TripDetailsDto MapToDetailsDto(Trip trip) => new()
        {
            Id = trip.Id,
            Name = trip.Name,
            Description = trip.Description,
            StartDate = trip.StartDate,
            EndDate = trip.EndDate,
            PlannedBudget = trip.PlannedBudget,
            UserId = trip.UserId,
            TotalExpenses = trip.Expenses?.Sum(e => e.Amount) ?? 0,
            RemainingBudget = trip.PlannedBudget - (trip.Expenses?.Sum(e => e.Amount) ?? 0),
            Destinations = trip.Destinations?.Select(d => new DestinationDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Notes = d.Notes,
                Location = d.Location,
                ArrivingDate = d.ArrivingDate,
                LeavingDate = d.LeavingDate,
                TripId = d.TripId,
                Activities = d.Activities?.Select(a => new Interfaces.DTOs.Activity.ActivityDto
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
            }) ?? [],
            Expenses = trip.Expenses?.Select(e => new ExpenseDto
            {
                Id = e.Id,
                Name = e.Name,
                Category = e.Category,
                Amount = e.Amount,
                Description = e.Description,
                CreatedAt = e.CreatedAt,
                TripId = e.TripId
            }) ?? []
        };
    }
}
