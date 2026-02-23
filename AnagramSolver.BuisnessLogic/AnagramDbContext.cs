using Microsoft.EntityFrameworkCore;
using AnagramSolver.Contracts;

namespace AnagramSolver.BusinessLogic
{
    public class AnagramDbContext : DbContext
    {
        public AnagramDbContext(DbContextOptions<AnagramDbContext> options) : base(options) { }

        public DbSet<WordEntity> Words { get; set; }
        public DbSet<WordGroupsEntity> WordGroupsEntity { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WordEntity>(entity =>
            {
                entity.Property(e => e.Value).HasMaxLength(100).IsRequired();
               
            });

            modelBuilder.Entity<WordGroupsEntity>(entity =>
            {
                entity.HasKey(e => e.Signature);
                entity.Property(e => e.Signature)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Words).IsRequired();
            });
        }



    }
}
