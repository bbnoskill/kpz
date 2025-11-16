using Microsoft.Extensions.Configuration;

namespace GamifiedLearningPlatform.Configuration;

public static class AppConfigurationFactory
{
    public static (DataAccessOptions dataAccess, SeedOptions seedOptions, IConfigurationRoot configurationRoot) Build()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var dataAccess = new DataAccessOptions();
        configuration.GetSection(DataAccessOptions.SectionName).Bind(dataAccess);

        var seed = new SeedOptions();
        configuration.GetSection(SeedOptions.SectionName).Bind(seed);

        return (dataAccess, seed, configuration);
    }
}



