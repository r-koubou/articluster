using System.IO;

using ArtiCluster.Applications.Services.Abstractions.Strategies;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Applications.Services.LocalFileConversions.Strategies;

public sealed class StudioOneLocalOutputNamingStrategy
    : ILocalOutputNamingStrategy<UniversalDefinition>
{
    public string GetOutputDirectory( string baseDirectory, UniversalDefinition source )
        => Path.Combine( baseDirectory, "StudioOne", source.ManufacturerName.Value, source.ProductName.Value );

    public string GetOutputFileName( UniversalDefinition source )
        => source.PatchName.Value + ".keyswitch";
}
