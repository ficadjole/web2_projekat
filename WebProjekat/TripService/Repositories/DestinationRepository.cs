using Microsoft.EntityFrameworkCore;
using TripService.DatabaseContext;
using TripService.Interfaces;
using TripService.Models;

namespace TripService.Repositories
{
    public class DestinationRepository : IDestinationRepository
    {
        private readonly TripsDbContext _context;

        public DestinationRepository(TripsDbContext context)
        {
            _context = context;
        }

        public async Task<Destination?> GetByIdAsync(Guid id)
            => await _context.Destinations.FindAsync(id);

        public async Task<Destination?> GetByIdWithActivitiesAsync(Guid id) => await _context.Destinations
                                                                                    .Include(d => d.Activities)
                                                                                    .FirstOrDefaultAsync(d => d.Id == id);

        public async Task<IEnumerable<Destination>> GetByTripIdAsync(Guid tripId) => await _context.Destinations
                                                                                            .Where(d => d.TripId == tripId)
                                                                                            .Include(d => d.Activities)
                                                                                            .ToListAsync();

        public async Task AddAsync(Destination destination)
        {
            await _context.Destinations.AddAsync(destination);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Destination destination)
        {
            _context.Destinations.Update(destination);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var destination = await GetByIdAsync(id);
            if (destination is null) return;

            _context.Destinations.Remove(destination);
            await _context.SaveChangesAsync();
        }
    }
}
