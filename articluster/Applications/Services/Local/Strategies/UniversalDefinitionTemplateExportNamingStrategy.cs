using ArtiCluster.Applications.Services.Abstractions.Strategies;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Applications.Services.Local.Strategies;

public sealed class UniversalDefinitionTemplateExportNamingStrategy
    : IExportNamingStrategy<UniversalDefinition>
{
    public string GetOutputDirectory( string baseDirectory, UniversalDefinition source )
        => baseDirectory;

    public string GetOutputFileName( UniversalDefinition source )
        => source.PatchName.Value + ".yaml";
}
