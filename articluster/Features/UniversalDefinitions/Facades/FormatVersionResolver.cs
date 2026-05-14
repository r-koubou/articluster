using ArtiCluster.Features.UniversalDefinitions.Contracts;
using ArtiCluster.Features.UniversalDefinitions.v1.Exports;
using ArtiCluster.Features.UniversalDefinitions.v1.Imports;

using Semver;

namespace ArtiCluster.Features.UniversalDefinitions.Facades;

internal static class FormatVersionResolver
{
    #region Latest version accessors
    public static IUniversalDefinitionImporter GetLatestImporter()
        => new YamlImporter();

    public static IUniversalDefinitionExporter GetLatestExporter()
        => new YamlExporter();
    #endregion ~Latest version accessors

    #region Version-specific resolvers
    public static IUniversalDefinitionImporter ResolveImporter( SemVersion formatVersion )
    {
        // For now, only have one importer, so we ignore the formatVersion.
        return new YamlImporter();
    }

    public static IUniversalDefinitionExporter ResolveExporter( SemVersion formatVersion )
    {
        // For now, only have one exporter, so we ignore the formatVersion.
        return new YamlExporter();
    }
    #endregion ~Version-specific resolvers
}
