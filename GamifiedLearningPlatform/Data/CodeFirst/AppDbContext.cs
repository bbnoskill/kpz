using GamifiedLearningPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace GamifiedLearningPlatform.Data.CodeFirst;

public class AppDbContext : DbContext
{
    public DbSet<Student> Students => Set<Student>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().ToTable("Students");
    }
}