using System.IO;

using ArtiCluster.Applications.Services.Abstractions.Imports.Strategies;

namespace ArtiCluster.Applications.Services.Local.Imports.Strategies;

public sealed class UniversalDefinitionImportNamingStrategy : IImportNamingStrategy
{
    public string GetInputFilePattern()
        => "*.yaml";

    public SearchOption GetSearchOption()
        => SearchOption.AllDirectories;
}
