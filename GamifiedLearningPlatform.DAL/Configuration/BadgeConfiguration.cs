using GamifiedLearningPlatform.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamifiedLearningPlatform.DAL.Configuration;

public class BadgeConfiguration : IEntityTypeConfiguration<Badge>
{
    public void Configure(EntityTypeBuilder<Badge> builder)
    {
        builder.HasKey(e => e.Id)
            .HasName("PK__Badges__3214EC0789A1B2C3");

        builder.Property(e => e.Id)
            .HasDefaultValueSql("NEWID()");

        builder.Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.IconUrl)
            .HasMaxLength(500);

        builder.Property(e => e.XpRequired)
            .HasDefaultValue(0);

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.HasIndex(e => e.Name)
            .IsUnique()
            .HasDatabaseName("UQ__Badges__Name");
    }
}