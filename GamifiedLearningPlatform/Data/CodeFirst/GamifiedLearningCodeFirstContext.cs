using GamifiedLearningPlatform.Data.CodeFirst.Entities;
using Microsoft.EntityFrameworkCore;

namespace GamifiedLearningPlatform.Data.CodeFirst;

public class GamifiedLearningCodeFirstContext : DbContext
{
    public GamifiedLearningCodeFirstContext(DbContextOptions<GamifiedLearningCodeFirstContext> options) : base(options)
    {
    }

    public DbSet<StudentEntity> Students => Set<StudentEntity>();
    public DbSet<AssignmentEntity> Assignments => Set<AssignmentEntity>();
    public DbSet<StudentBadgeEntity> StudentBadges => Set<StudentBadgeEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<StudentEntity>(entity =>
        {
            entity.ToTable("Students");
            entity.HasIndex(x => x.Email).IsUnique();
            entity.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Email).IsRequired().HasMaxLength(200);
            entity.Property(x => x.TotalXp).HasDefaultValue(0);
            entity.Property(x => x.Level).HasDefaultValue(1);
        });

        modelBuilder.Entity<AssignmentEntity>(entity =>
        {
            entity.ToTable("Assignments");
            entity.Property(x => x.Title).IsRequired().HasMaxLength(200);
            entity.HasOne(x => x.Student)
                .WithMany(x => x.Assignments)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<StudentBadgeEntity>(entity =>
        {
            entity.ToTable("StudentBadges");
            entity.Property(x => x.Name).IsRequired().HasMaxLength(150);
            entity.HasOne(x => x.Student)
                .WithMany(x => x.Badges)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}



