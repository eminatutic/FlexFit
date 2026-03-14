using Microsoft.EntityFrameworkCore;
using FlexFit.Models;

namespace FlexFit.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<FitnessObject> FitnessObjects { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<MembershipCard> MembershipCards { get; set; }

        public DbSet<DailyCard> DailyCards { get; set; }

        public DbSet<SubscriptionCard> SubscriptionCards { get; set; }

        public DbSet<PenaltyCard> PenaltyCards { get; set; }

        public DbSet<PenaltyPoint> PenaltyPoints { get; set; }

        public DbSet<Reservation> Reservations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // FitnessObject -> Resources (1:N)
            modelBuilder.Entity<Resource>()
                .HasOne(r => r.FitnessObject)
                .WithMany(f => f.Resources)
                .HasForeignKey(r => r.FitnessObjectId);

            // Member -> Reservations (1:N)
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Member)
                .WithMany(m => m.Reservations)
                .HasForeignKey(r => r.MemberId);

            // Resource -> Reservations (1:N)
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Resource)
                .WithMany()
                .HasForeignKey(r => r.ResourceId);

            // Member -> PenaltyPoints (1:N)
            modelBuilder.Entity<PenaltyPoint>()
                .HasOne(p => p.Member)
                .WithMany(m => m.PenaltyPointHistory)
                .HasForeignKey(p => p.MemberId);

            // Member -> PenaltyCards (1:N)
            modelBuilder.Entity<PenaltyCard>()
                .HasOne(p => p.Member)
                .WithMany()
                .HasForeignKey(p => p.MemberId);

            // FitnessObject -> PenaltyCards (1:N)
            modelBuilder.Entity<PenaltyCard>()
                .HasOne(p => p.FitnessObject)
                .WithMany()
                .HasForeignKey(p => p.FitnessObjectId);

            // DailyCard -> FitnessObject (1:N)
            modelBuilder.Entity<DailyCard>()
                .HasOne(d => d.FitnessObject)
                .WithMany()
                .HasForeignKey(d => d.FitnessObjectId);

            // MembershipCard -> Member (1:N)
            modelBuilder.Entity<MembershipCard>()
                .HasOne(c => c.Member)
                .WithMany()
                .HasForeignKey(c => c.MemberId);
        }
    }
}