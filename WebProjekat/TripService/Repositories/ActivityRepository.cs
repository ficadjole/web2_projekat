using Microsoft.EntityFrameworkCore;
using TripService.DatabaseContext;
using TripService.Interfaces;
using TripService.Models;

namespace TripService.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly TripsDbContext _context;

        public ActivityRepository(TripsDbContext context)
        {
            _context = context;
        }

        public async Task<Activity?> GetByIdAsync(Guid id) => await _context.Activities.FindAsync(id);

        public async Task<IEnumerable<Activity>> GetByDestinationIdAsync(Guid destinationId) => await _context.Activities
                                                                                                    .Where(a => a.DestinationId == destinationId)
                                                                                                    .OrderBy(a => a.Date)
                                                                                                    .ToListAsync();

        public async Task<IEnumerable<Activity>> GetByDateAsync(Guid tripId, DateTime date) => await _context.Activities
                                                                                                    .Include(a => a.Destination)
                                                                                                    .Where(a => a.Destination.TripId == tripId && a.Date.Date == date.Date)
                                                                                                    .OrderBy(a => a.Date)
                                                                                                    .ToListAsync();
        public async Task AddAsync(Activity activity)
        {
            await _context.Activities.AddAsync(activity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Activity activity)
        {
            _context.Activities.Update(activity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var activity = await GetByIdAsync(id);
            if (activity is null) return;

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
        }
    }
}
