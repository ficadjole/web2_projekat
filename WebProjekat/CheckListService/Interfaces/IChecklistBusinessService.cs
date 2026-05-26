
using CheckListService.Interfaces.DTOs;
using WebProjekat.Common;

namespace CheckListService.Interfaces
{
    public interface IChecklistBusinessService
    {

        Task<IEnumerable<ChecklistDto>> GetAllAsync();
        Task<Result<ChecklistDto>> GetByTripIdAsync(Guid tripId, Guid userId);
        Task<Result<ChecklistItemDto>> AddItemAsync(Guid itemId,CreateChecklistItemDto dto, Guid userId);
        Task<Result<ChecklistItemDto>> ToggleItemAsync(Guid itemId, Guid userId);
        Task<Result> DeleteItemAsync(Guid itemId, Guid userId);
        Task<Result> DeleteChecklistAsync(Guid checklistId, Guid userId);

    }
}
