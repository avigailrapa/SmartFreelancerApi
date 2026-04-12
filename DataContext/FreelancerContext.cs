using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.interfaces;

namespace FreelancersApi.DataContext
{
    public class FreelancerContext(DbContextOptions<FreelancerContext> options) : DbContext(options), IContext
    {

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Freelancer> Freelancers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Proposal> Proposals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Freelancer>()
                .Property(f => f.HourlyRate)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Job>()
                .Property(j => j.MaxPayPerHour)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Proposal>()
                .Property(p => p.HourlyRate)
                .HasColumnType("decimal(18,2)");

            // --- קשרים עבור Ratings ---
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.RatingsGiven)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Freelancer)
                .WithMany(f => f.RatingsReceived)
                .HasForeignKey(r => r.FreelancerId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- קשרים עבור Proposals ---
            modelBuilder.Entity<Proposal>(entity =>
            {
                entity.HasOne(p => p.Job)
                    .WithMany(j => j.Proposals)
                    .HasForeignKey(p => p.JobId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Freelancer)
                    .WithMany(f => f.ProposalsSubmitted)
                    .HasForeignKey(p => p.FreelancerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // Skills
            modelBuilder.Entity<Freelancer>()
                .HasMany(f => f.Skills)
                .WithMany(c => c.SkillFreelancers)
                .UsingEntity(j => j.ToTable("FreelancerSkills"));

            // Specializations
            modelBuilder.Entity<Freelancer>()
                .HasMany(f => f.Specializations)
                .WithMany(c => c.SpecializationFreelancers)
                .UsingEntity(j => j.ToTable("FreelancerSpecializations"));

            modelBuilder.Entity<Freelancer>()
             .HasOne(f => f.MainCategory)
            .WithMany()
            .HasForeignKey(f => f.MainCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

            // --- קשרים עבור Job ---
            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasOne(j => j.MainCategory)
                    .WithMany(c => c.Jobs)
                    .HasForeignKey(j => j.MainCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(j => j.RequiredSkills)
                    .WithMany()
                    .UsingEntity(j => j.ToTable("JobRequiredSkills"));
            });

            // --- היררכיית קטגוריות ---
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasOne(c => c.ParentCategory)
                    .WithMany(c => c.SubCategories)
                    .HasForeignKey(c => c.ParentCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        public async Task Save() => await SaveChangesAsync();
    }
}