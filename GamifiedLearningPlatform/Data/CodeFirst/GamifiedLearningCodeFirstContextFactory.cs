using GamifiedLearningPlatform.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GamifiedLearningPlatform.Data.CodeFirst;

public class GamifiedLearningCodeFirstContextFactory : IDesignTimeDbContextFactory<GamifiedLearningCodeFirstContext>
{
    public GamifiedLearningCodeFirstContext CreateDbContext(string[] args)
    {
        var (_, _, configuration) = AppConfigurationFactory.Build();
        var connectionString = configuration.GetSection($"{DataAccessOptions.SectionName}:ConnectionStrings:CodeFirst").Get<string>()
                              ?? throw new InvalidOperationException("CodeFirst connection string is missing.");

        var optionsBuilder = new DbContextOptionsBuilder<GamifiedLearningCodeFirstContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new GamifiedLearningCodeFirstContext(optionsBuilder.Options);
    }
}

