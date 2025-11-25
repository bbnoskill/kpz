using GamifiedLearningPlatform.DAL.Configuration;
using GamifiedLearningPlatform.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GamifiedLearningPlatform.DAL.Context;
public class ApplicationDbContext : DbContext
{
    // Конструктор для Dependency Injection
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Конструктор без параметрів для міграцій
    public ApplicationDbContext()
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Badge> Badges { get; set; }
    public DbSet<StudentBadge> StudentBadges { get; set; }
    public DbSet<Assignment> Assignments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Застосовуємо конфігурації
        modelBuilder.ApplyConfiguration(new StudentConfiguration());
        modelBuilder.ApplyConfiguration(new BadgeConfiguration());
        modelBuilder.ApplyConfiguration(new StudentBadgeConfiguration());
        modelBuilder.ApplyConfiguration(new AssignmentConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // ЗМІНІТЬ ЦЕЙ РЯДОК:
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=GamifiedLearning_CodeFirst;Trusted_Connection=true;TrustServerCertificate=true;");
        }
    }
}