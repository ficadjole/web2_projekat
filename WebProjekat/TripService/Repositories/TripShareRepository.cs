
using Microsoft.EntityFrameworkCore;
using TripService.DatabaseContext;
using TripService.Interfaces;
using TripService.Models;

namespace TripService.Repositories
{
    public class TripShareRepository : ITripShareRepository
    {
        private readonly TripsDbContext _context;

        public TripShareRepository(TripsDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TripShare tripShare)
        {
            await _context.TripShares.AddAsync(tripShare);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var tripShare = await GetByIdAsync(id);

            if (tripShare is null) return;

            _context.TripShares.Remove(tripShare);

            await _context.SaveChangesAsync();
        }

        public async Task<TripShare?> GetByIdAsync(Guid id) => await _context.TripShares.FindAsync(id);

        public async Task<TripShare?> GetByTokenAsync(string token) => await _context.TripShares.Include(ts => ts.Trip)
                                                                                                .FirstOrDefaultAsync(ts => ts.Token == token);

        public async Task<IEnumerable<TripShare>> GetByTripIdAsync(Guid tripId) => await _context.TripShares.Where(ts => ts.TripId == tripId).ToListAsync();
    }
}
