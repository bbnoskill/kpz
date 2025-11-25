using GamifiedLearningPlatform.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamifiedLearningPlatform.DAL.Configuration;

public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.HasKey(e => e.Id)
            .HasName("PK__Assignments__3214EC07A1B2C3D4");

        builder.Property(e => e.Id)
            .HasDefaultValueSql("NEWID()");

        builder.Property(e => e.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.XpAward)
            .IsRequired();

        builder.Property(e => e.IsCompleted)
            .HasDefaultValue(false);

        builder.HasOne(a => a.Student)
            .WithMany(s => s.Assignments)
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Assignments_Students");
    }
}