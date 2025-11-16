using GamifiedLearningPlatform.Configuration;
using GamifiedLearningPlatform.Data.CodeFirst;
using GamifiedLearningPlatform.Data.DbFirst;
using Microsoft.EntityFrameworkCore;

namespace GamifiedLearningPlatform.Data.Repositories;

public static class StudentRepositoryFactory
{
    public static IStudentRepository Create(DataAccessOptions options)
    {
        return options.DefaultProvider switch
        {
            DataProviderNames.AdoNet => new StudentRepositoryWithAdoNet(options.ConnectionStrings.AdoNet),
            DataProviderNames.DbFirstEf => CreateDbFirstRepository(options.ConnectionStrings.DbFirst),
            _ => CreateCodeFirstRepository(options.ConnectionStrings.CodeFirst)
        };
    }

    private static IStudentRepository CreateCodeFirstRepository(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("CodeFirst connection string was not provided.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<GamifiedLearningCodeFirstContext>()
            .UseSqlServer(connectionString);

        var factory = new OptionsDbContextFactory<GamifiedLearningCodeFirstContext>(optionsBuilder.Options);
        return new EfCodeFirstStudentRepository(factory);
    }

    private static IStudentRepository CreateDbFirstRepository(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("DbFirst connection string was not provided.");
        }

        var factory = new DbFirstContextFactory(connectionString);
        return new EfDbFirstStudentRepository(factory);
    }
}

