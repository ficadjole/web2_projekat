using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace UserService.DatabaseContext
{
    public class UsersDbContextFactory : IDesignTimeDbContextFactory<UsersDbContext>
    {
        public UsersDbContext CreateDbContext(string[] args)
        {


            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<UsersDbContext>();

            var connectionString = configuration["Values:SqlConnectionString"];

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'SqlConnectionString' not found.");
            }

            optionsBuilder.UseSqlServer(connectionString);

            return new UsersDbContext(optionsBuilder.Options);
        }
    }
}
