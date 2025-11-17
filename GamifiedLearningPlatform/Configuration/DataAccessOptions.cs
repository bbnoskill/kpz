namespace GamifiedLearningPlatform.Configuration;

public class DataAccessOptions
{
    public const string SectionName = "DataAccess";
    public string DefaultProvider { get; set; } = DataProviderNames.CodeFirstEf;
    public ConnectionStringsOptions ConnectionStrings { get; set; } = new();
}

public static class DataProviderNames
{
    public const string CodeFirstEf = "CodeFirstEf";
    public const string DbFirstEf = "DbFirstEf";
}

public class ConnectionStringsOptions
{
    public string CodeFirst { get; set; } = string.Empty;
    public string DbFirst { get; set; } = string.Empty;
}





