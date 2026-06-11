
using Microsoft.EntityFrameworkCore;
using TripService.DatabaseContext;
using TripService.Interfaces;
using TripService.Models;

namespace TripService.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly TripsDbContext _context;

        public TripRepository(TripsDbContext context)
        {
            _context = context;
        }

        public async Task<Trip?> GetByIdAsync(Guid id)
            => await _context.Trips.FindAsync(id);

        //Vraca Trip sa svim detaljima kao sto su Aktivnosti i Destinacije i Troskovi
        public async Task<Trip?> GetByIdWithDetailsAsync(Guid id) => await _context.Trips
                                                                            .Include(t => t.Destinations)
                                                                            .ThenInclude(d => d.Activities)
                                                                            .Include(t => t.Expenses)
                                                                            .FirstOrDefaultAsync(t => t.Id == id);

        public async Task<IEnumerable<Trip>> GetAllByUserIdAsync(Guid userId) => await _context.Trips
                                                                            .Where(t => t.UserId == userId)
                                                                            .ToListAsync();

        public async Task<IEnumerable<Trip>> GetAllAsync() => await _context.Trips.ToListAsync();

        public async Task AddAsync(Trip trip)
        {
            await _context.Trips.AddAsync(trip);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Trip trip)
        {
            _context.Trips.Update(trip);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var trip = await GetByIdAsync(id);
            if (trip is null) return;

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();
        }
    }
}
