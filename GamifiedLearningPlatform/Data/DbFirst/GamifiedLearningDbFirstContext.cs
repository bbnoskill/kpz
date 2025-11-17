using GamifiedLearningPlatform.Data.DbFirst.Entities;
using Microsoft.EntityFrameworkCore;

namespace GamifiedLearningPlatform.Data.DbFirst;

public partial class GamifiedLearningDbFirstContext : DbContext
{
    public GamifiedLearningDbFirstContext(DbContextOptions<GamifiedLearningDbFirstContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Student> Students => Set<Student>();
    public virtual DbSet<Assignment> Assignments => Set<Assignment>();
    public virtual DbSet<StudentBadge> StudentBadges => Set<StudentBadge>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Students");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(200);

            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.ToTable("Assignments");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Student)
                .WithMany(p => p.Assignments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<StudentBadge>(entity =>
        {
            entity.ToTable("StudentBadges");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(150);

            entity.HasOne(d => d.Student)
                .WithMany(p => p.StudentBadges)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}





