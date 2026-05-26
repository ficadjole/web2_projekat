using CheckListService.DatabaseContext;
using CheckListService.Interfaces;
using CheckListService.Models;

namespace CheckListService.Repositories
{
    public class ChecklistItemRepository : IChecklistItemRepository
    {
        private readonly CheckListsDbContext _context;

        public ChecklistItemRepository(CheckListsDbContext context)
        {
            _context = context;
        }

        public async Task AddItemAsync(ChecklistItem item)
        {
            await _context.ChecklistItem.AddAsync(item);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(Guid itemId)
        {
            var item = await GetItemByIdAsync(itemId);

            if (item is null) return;

            _context.ChecklistItem.Remove(item);

            await _context.SaveChangesAsync();
        }

        public async Task<ChecklistItem?> GetItemByIdAsync(Guid itemId) => await _context.ChecklistItem.FindAsync(itemId);


        public async Task UpdateItemAsync(ChecklistItem item)
        {
            _context.ChecklistItem.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}
