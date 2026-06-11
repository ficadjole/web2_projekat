using CheckListService.Interfaces;
using Common.Enums;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using TripService.Documents;
using TripService.Documents.Model;
using TripService.Interfaces;
using WebProjekat.Common;

namespace TripService.Services
{
    public class PdfService(ITripRepository tripRepository, IExpenseRepository expenseRepository) : IPdfService
    {
        public async Task<Result<byte[]>> GenerateTripReportAsync(Guid tripId, Guid userId)
        {
            var trip = await tripRepository.GetByIdWithDetailsAsync(tripId);
            if (trip is null)
                return Result<byte[]>.Failure("Trip not found.", ErrorType.NotFound);

            if (trip.UserId != userId)
                return Result<byte[]>.Failure("Unauthorized.", ErrorType.Unauthorized);

            var expenses = await expenseRepository.GetByTripIdAsync(tripId);

            var proxy = ServiceProxy.Create<IChecklistService>(new Uri("fabric:/WebProjekat/CheckListService"), new ServicePartitionKey(0));

            var checklist = await proxy.GetByTripIdAsync(tripId, userId);

            if (checklist is null)
                return Result<byte[]>.Failure("Checklist not found.", ErrorType.NotFound);

            var reportData = new TripReportData
            {
                Trip = trip,
                Destinations = trip.Destinations.Select(d => new DestinationWithActivities
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Notes = d.Notes,
                    Location = d.Location,
                    ArrivingDate = d.ArrivingDate,
                    LeavingDate = d.LeavingDate,
                    TripId = d.TripId,
                    Activities = d.Activities?.ToList() ?? new()
                }).ToList(),
                Expenses = expenses.ToList(),
                ChecklistItems = checklist.Value?.Items?.ToList() ?? new()
            };

            QuestPDF.Settings.License = LicenseType.Community;

            var document = new TripReportDocument(reportData);
            var bytes = document.GeneratePdf();

            return Result<byte[]>.Success(bytes);
        }
    }
}
