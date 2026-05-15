using ArtiCluster.Features.UniversalDefinitions.Contracts;
using ArtiCluster.Features.UniversalDefinitions.v1.Exports;
using ArtiCluster.Features.UniversalDefinitions.v1.Imports;

using Semver;

namespace ArtiCluster.Features.UniversalDefinitions.Facades;

internal static class FormatVersionResolver
{
    #region Latest version accessors
    public static IUniversalDefinitionExporter GetLatestExporter()
        => new YamlExporter();
    #endregion ~Latest version accessors

    #region Version-specific resolvers
    public static IUniversalDefinitionImporter ResolveImporter( SemVersion formatVersion )
    {
        // For now, only have one importer, so we ignore the formatVersion.
        return new YamlImporter();
    }
    #endregion ~Version-specific resolvers
}
