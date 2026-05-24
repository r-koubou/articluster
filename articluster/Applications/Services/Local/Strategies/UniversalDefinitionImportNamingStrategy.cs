using System.IO;

using ArtiCluster.Applications.Services.Abstractions.Strategies;

namespace ArtiCluster.Applications.Services.Local.Strategies;

public sealed class UniversalDefinitionImportNamingStrategy : IImportNamingStrategy
{
    public string GetInputFilePattern()
        => "*.yaml";

    public SearchOption GetSearchOption()
        => SearchOption.AllDirectories;
}
