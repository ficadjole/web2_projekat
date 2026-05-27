using CheckListService.DatabaseContext;
using CheckListService.Interfaces;
using CheckListService.Models;
using Microsoft.EntityFrameworkCore;

namespace CheckListService.Repositories
{
    public class ChecklistRepository : IChecklistRepository
    {
        private readonly CheckListsDbContext _context;

        public ChecklistRepository(CheckListsDbContext context)
        {
            _context = context;
        }

        public async Task<Checklist> CreateChecklistAsync(Checklist checklist)
        {
            await _context.Checklists.AddAsync(checklist);
            await _context.SaveChangesAsync();
            return checklist;
        }

        public async Task DeleteChecklistAsync(Guid tripId)
        {

            var checklist = await GetByTripIdAsync(tripId);

            if (checklist is null) return;

            _context.Checklists.Remove(checklist);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Checklist>> GetAllAsync() => await _context.Checklists
                                                                                .Include(c => c.Items)
                                                                                .ToListAsync();

        public async Task<Checklist?> GetByIdAsync(Guid id) => await _context.Checklists
                                                                            .Include(c => c.Items)
                                                                            .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Checklist?> GetByTripIdAsync(Guid tripId) => await _context.Checklists
                                                                                    .Include(c => c.Items)
                                                                                    .FirstOrDefaultAsync(c => c.TripId == tripId);
    }
}
