using CheckListService.Interfaces.DTOs;
using Microsoft.ServiceFabric.Services.Remoting;
using WebProjekat.Common;

namespace CheckListService.Interfaces
{
    public interface IChecklistService : IService
    {
        Task<Result<ChecklistDto>> GetByTripIdAsync(Guid tripId, Guid userId);
        Task<Result<ChecklistItemDto>> AddItemAsync(CreateChecklistItemDto dto, Guid userId);
        Task<Result<ChecklistItemDto>> ToggleItemAsync(Guid tripId,Guid itemId, Guid userId);
        Task<Result> DeleteChecklistAsync(Guid tripId, Guid userId);
        Task<Result> DeleteItemAsync(Guid tripId, Guid itemId, Guid userId);

    }
}
