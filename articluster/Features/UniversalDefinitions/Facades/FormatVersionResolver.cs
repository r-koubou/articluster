using ArtiCluster.Features.UniversalDefinitions.Contracts;
using ArtiCluster.Features.UniversalDefinitions.Exports;
using ArtiCluster.Features.UniversalDefinitions.Imports;

namespace ArtiCluster.Features.UniversalDefinitions.Facades;

/// <summary>
/// Now, only have one importer, so we ignore the formatVersion.
/// </summary>
/// <remarks>
/// In the future, if we have multiple importers, we'll need to determine the formatVersion first (probably by peeking at the content)
/// and then resolve the appropriate importer.
/// </remarks>
internal static class FormatVersionResolver
{
    #region Latest version accessors
    public static IUniversalDefinitionExporter GetLatestExporter()
        => new YamlExporter();
    #endregion ~Latest version accessors

    #region Version-specific resolvers
    /// <summary>
    /// Currently, it always returns a <see cref="YamlImporter"/> instance.
    /// </summary>
    public static IUniversalDefinitionImporter ResolveImporter( int formatVersion )
    {
        // For now, only have one importer, so we ignore the formatVersion.
        return new YamlImporter();
    }
    #endregion ~Version-specific resolvers
}
