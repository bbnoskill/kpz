using Microsoft.EntityFrameworkCore;

namespace GamifiedLearningPlatform.Data.DbFirst;

public class DbFirstContextFactory : IDbContextFactory<GamifiedLearningDbFirstContext>
{
    private readonly DbContextOptions<GamifiedLearningDbFirstContext> _options;

    public DbFirstContextFactory(string connectionString)
    {
        var builder = new DbContextOptionsBuilder<GamifiedLearningDbFirstContext>()
            .UseSqlServer(connectionString);

        _options = builder.Options;
    }

    public GamifiedLearningDbFirstContext CreateDbContext()
    {
        return new GamifiedLearningDbFirstContext(_options);
    }

    public Task<GamifiedLearningDbFirstContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(CreateDbContext());
    }
}





