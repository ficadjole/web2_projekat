using Microsoft.EntityFrameworkCore;
using TripService.Models;

namespace TripService.DatabaseContext
{
    public class TripsDbContext : DbContext
    {
        public TripsDbContext(DbContextOptions<TripsDbContext> options) : base(options)
        {
        }

        public TripsDbContext()
        {
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Destination> Destinations { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<Expense> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TripsDbContext).Assembly);

            modelBuilder.Entity<Trip>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("newid()");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate).IsRequired();
                entity.Property(e => e.PlannedBudget).IsRequired().HasPrecision(18, 2);
                entity.Property(e => e.UserId).IsRequired();

                entity.HasMany(e => e.Destinations)
                      .WithOne(d => d.Trip)
                      .HasForeignKey(d => d.TripId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Expenses)
                      .WithOne(ex => ex.Trip)
                      .HasForeignKey(ex => ex.TripId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Destination>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("newid()");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Notes).HasMaxLength(500);
                entity.Property(e => e.Location).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ArrivingDate).IsRequired();
                entity.Property(e => e.LeavingDate).IsRequired();
                entity.Property(e => e.TripId).IsRequired();

                entity.HasMany(e => e.Activities)
                      .WithOne(a => a.Destination)
                      .HasForeignKey(a => a.DestinationId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("newid()");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Location).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.EstimatedCost).HasPrecision(18, 2);
                entity.Property(e => e.Status).IsRequired().HasConversion<string>();
                entity.Property(e => e.DestinationId).IsRequired();
            });

            modelBuilder.Entity<Expense>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("newid()");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Category).IsRequired().HasConversion<string>();
                entity.Property(e => e.Amount).IsRequired().HasPrecision(18, 2);
                entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("getutcdate()");
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.TripId).IsRequired();
            });
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = Environment.GetEnvironmentVariable("Values:SqlConnectionString");

                if (!string.IsNullOrEmpty(connectionString))
                {
                    optionsBuilder.UseSqlServer(connectionString);
                }
            }
        }
    }
}
