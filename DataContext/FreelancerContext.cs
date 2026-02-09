using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.interfaces;


namespace FreelancersApi.DataContext
{
    public class FreelancerContext(string connection) : DbContext, IContext
    {
        private readonly string connection = connection;

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Freelancer> Freelancers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Proposal> Proposals { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connection);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // קשר בין Rating ל-User
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.RatingsGiven)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // קשר בין Rating ל-Freelancer
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Freelancer)
                .WithMany(f => f.RatingsReceived)
                .HasForeignKey(r => r.FreelancerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Proposal>(entity =>
            {
                entity.HasOne(p => p.Job)
                    .WithMany(j => j.Proposals)
                    .HasForeignKey(p => p.JobId)
                    .OnDelete(DeleteBehavior.Restrict); // פתרון בעיית המעגליות

                entity.HasOne(p => p.Freelancer)
                    .WithMany(f => f.ProposalsSubmitted)
                    .HasForeignKey(p => p.FreelancerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }

        public async Task Save()
        {
            await SaveChangesAsync();
        }


    }
}
