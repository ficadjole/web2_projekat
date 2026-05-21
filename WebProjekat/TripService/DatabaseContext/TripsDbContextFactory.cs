using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TripService.DatabaseContext
{
    public class TripsDbContextFactory : IDesignTimeDbContextFactory<TripsDbContext>
    {
        public TripsDbContext CreateDbContext(string[] args)
        {


            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TripsDbContext>();

            var connectionString = configuration["Values:SqlConnectionString"];

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'SqlConnectionString' not found.");
            }

            optionsBuilder.UseSqlServer(connectionString);

            return new TripsDbContext(optionsBuilder.Options);
        }
    }
}
