using System;
using System.Collections.Generic;
using GastappApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GastappApi;

public partial class GastappDbContext : DbContext
{
    public GastappDbContext()
    {
    }

    public GastappDbContext(DbContextOptions<GastappDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Spending> Spending { get; set; }

    public virtual DbSet<SpendingCategory> SpendingCategories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:gastappserver.database.windows.net,1433;Initial Catalog=gastapp;Persist Security Info=False;User ID=gastappadmin;Password=GaStAp_1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Spending>(entity =>
        {
            entity.HasKey(e => e.SpendingId).HasName("PK__Spending__79C1883678F3D470");

            entity.ToTable("Spending");

            entity.HasIndex(e => e.CategoryId, "IX_CategoryId");

            entity.Property(e => e.Amount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.SpendingDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Category).WithMany(p => p.Spendings)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Spending__Catego__5F7E2DAC");
        });

        modelBuilder.Entity<SpendingCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Spending__19093A0B9A0006CD");

            entity.ToTable("SpendingCategory");

            entity.HasIndex(e => e.UserId, "IX_UserId");

            entity.Property(e => e.CategoryName).HasMaxLength(100);
            

            entity.HasOne(d => d.User).WithMany(p => p.SpendingCategories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__SpendingC__UserI__5BAD9CC8");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CE0C62117");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
