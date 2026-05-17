using System.IO;

using ArtiCluster.Applications.Services.Abstractions.Local.Strategies;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Applications.Services.Local.Strategies;

public sealed class StudioOneLocalOutputNamingStrategy
    : ILocalOutputNamingStrategy<UniversalDefinition>
{
    public string GetOutputDirectory( string baseDirectory, UniversalDefinition source )
        => Path.Combine( baseDirectory, "StudioOne", source.ManufacturerName.Value, source.ProductName.Value );

    public string GetOutputFileName( UniversalDefinition source )
        => source.PatchName.Value + ".keyswitch";
}
