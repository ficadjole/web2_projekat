
using WebProjekat.Common;

namespace TripService.Interfaces
{
    public interface IPdfService
    {
        Task<Result<byte[]>> GenerateTripReportAsync(Guid tripId, Guid userId);
    }

}
