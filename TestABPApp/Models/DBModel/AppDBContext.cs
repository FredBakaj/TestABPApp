using Microsoft.EntityFrameworkCore;

namespace TestABPApp.Models.DBModel
{
    public class AppDBContext: DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<ABExperiment> ABExperiments { get; set; } = null!;
        public DbSet<ABExperimentValue> ABExperimentValues { get; set; } = null!;
        public DbSet<ABExperimentRecord> ABExperimentRecords { get; set; } = null!;

        public AppDBContext(DbContextOptions<AppDBContext> options)
        : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.DeviceToken);
                entity.Property(e => e.DeviceToken).ValueGeneratedNever();
                entity.Property(e => e.DateRegistration).IsRequired();
            });

            modelBuilder.Entity<ABExperiment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.DateStart).IsRequired();
            });

            modelBuilder.Entity<ABExperimentValue>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Value).IsRequired();
                entity.Property(e => e.GroupName).IsRequired();
                entity.Property(e => e.СoefficientPerformance).IsRequired();

                entity.HasOne(e => e.ABExperiment)
                    .WithMany()
                    .HasForeignKey(e => e.ABExperimentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ABExperimentRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Action).IsRequired();
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId);
                entity.HasOne(e => e.ABExperiment)
                    .WithMany()
                    .HasForeignKey(e => e.ABExperimentId);
                entity.HasOne(e => e.ABExperimentValue)
                    .WithMany()
                    .HasForeignKey(e => e.ABExperimentValueId);
            });
        }
    }
}
