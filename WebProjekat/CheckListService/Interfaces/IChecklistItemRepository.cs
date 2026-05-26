using CheckListService.Models;

namespace CheckListService.Interfaces
{
    public interface IChecklistItemRepository
    {
        Task<ChecklistItem?> GetItemByIdAsync(Guid itemId);
        Task AddItemAsync(ChecklistItem item);
        Task UpdateItemAsync(ChecklistItem item);
        Task DeleteItemAsync(Guid itemId);
    }
}
