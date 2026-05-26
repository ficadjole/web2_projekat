using CheckListService.Models;

namespace CheckListService.Interfaces
{
    public interface IChecklistRepository
    {
        Task<IEnumerable<Checklist>> GetAllAsync();
        Task<Checklist?> GetByIdAsync(Guid id);
        Task<Checklist?> GetByTripIdAsync(Guid tripId);
        Task<Checklist> CreateChecklistAsync(Checklist checklist);
        Task DeleteChecklistAsync(Guid checklistId);
    }
}
