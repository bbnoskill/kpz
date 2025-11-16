using Microsoft.EntityFrameworkCore;

namespace GamifiedLearningPlatform.Data.Repositories;

public class OptionsDbContextFactory<TContext> : IDbContextFactory<TContext>
    where TContext : DbContext
{
    private readonly DbContextOptions<TContext> _options;
    private readonly Func<DbContextOptions<TContext>, TContext> _contextFactory;

    public OptionsDbContextFactory(DbContextOptions<TContext> options, Func<DbContextOptions<TContext>, TContext>? contextFactory = null)
    {
        _options = options;
        _contextFactory = contextFactory ?? (opts => (TContext)Activator.CreateInstance(typeof(TContext), opts)!);
    }

    public TContext CreateDbContext()
    {
        return _contextFactory(_options);
    }

    public Task<TContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(CreateDbContext());
    }
}



