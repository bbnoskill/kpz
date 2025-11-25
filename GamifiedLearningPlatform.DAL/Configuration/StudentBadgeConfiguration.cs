using GamifiedLearningPlatform.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamifiedLearningPlatform.DAL.Configuration;

public class StudentBadgeConfiguration : IEntityTypeConfiguration<StudentBadge>
{
    public void Configure(EntityTypeBuilder<StudentBadge> builder)
    {
        builder.HasKey(e => e.Id)
            .HasName("PK__StudentBadges__3214EC07D4E5F2C7");

        builder.Property(e => e.Id)
            .HasDefaultValueSql("NEWID()");

        builder.Property(e => e.EarnedAt)
            .HasDefaultValueSql("GETDATE()");

        // Унікальний індекс для уникнення дублікатів
        builder.HasIndex(e => new { e.StudentId, e.BadgeId })
            .IsUnique()
            .HasDatabaseName("UQ__StudentBadges__StudentId_BadgeId");

        // Відносини
        builder.HasOne(sb => sb.Student)
            .WithMany(s => s.StudentBadges)
            .HasForeignKey(sb => sb.StudentId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_StudentBadges_Students");

        builder.HasOne(sb => sb.Badge)
            .WithMany(b => b.StudentBadges)
            .HasForeignKey(sb => sb.BadgeId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_StudentBadges_Badges");
    }
}