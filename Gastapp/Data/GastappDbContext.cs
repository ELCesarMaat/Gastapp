using Gastapp.Models;
using Gastapp.Utils;
using Microsoft.EntityFrameworkCore;

namespace Gastapp.Data
{
    public class GastappDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Spending> Spending { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionDb = $"Filename={SqliteDb.GetDbRoute()}";
            optionsBuilder.UseSqlite(connectionDb);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Spending>(entity =>
            {
                entity.HasKey(e => e.SpendingId).HasName("PK_Spending");

                entity.HasIndex(e => e.CategoryId, "IX_CategoryId");

                entity.Property(e => e.Amount)
                    .HasDefaultValue(0m)
                    .HasColumnType("decimal(18, 0)");
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.SpendingDate).HasColumnType("datetime");
                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Spendings)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Spending_Category");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId).HasName("PK_Category");
                entity.HasIndex(e => e.UserId, "IX_UserId");

                entity.Property(e => e.CategoryName).HasMaxLength(100);

                entity.HasMany(c => c.Spendings)
                    .WithOne(s => s.Category)
                    .HasForeignKey(s => s.CategoryId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
