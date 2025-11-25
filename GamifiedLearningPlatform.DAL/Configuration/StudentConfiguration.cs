using GamifiedLearningPlatform.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamifiedLearningPlatform.DAL.Configuration;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(e => e.Id)
            .HasName("PK__Students__3214EC075A4B3A2E");

        builder.Property(e => e.Id)
            .HasDefaultValueSql("NEWID()");

        builder.Property(e => e.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.TotalXp)
            .HasDefaultValue(0);

        builder.Property(e => e.Level)
            .HasDefaultValue(1);

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.HasIndex(e => e.Email)
            .IsUnique()
            .HasDatabaseName("UQ__Students__Email");
    }
}