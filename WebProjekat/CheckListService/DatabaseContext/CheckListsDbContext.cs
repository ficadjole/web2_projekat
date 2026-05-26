using CheckListService.Models;
using Microsoft.EntityFrameworkCore;

namespace CheckListService.DatabaseContext
{
    public class CheckListsDbContext : DbContext
    {

        public CheckListsDbContext(DbContextOptions<CheckListsDbContext> options) : base(options) { }

        public CheckListsDbContext() { }

        public DbSet<Checklist> Checklists { get; set; }

        public DbSet<ChecklistItem> ChecklistItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CheckListsDbContext).Assembly);

            modelBuilder.Entity<Checklist>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("newid()");
                entity.Property(e => e.TripId).IsRequired();
                entity.Property(e => e.UserId).IsRequired();

                entity.HasMany(e => e.Items)
                      .WithOne(i => i.Checklist)
                      .HasForeignKey(i => i.ChecklistId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ChecklistItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("newid()");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.IsChecked).HasDefaultValue(false);
                entity.Property(e => e.ChecklistId).IsRequired();
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
