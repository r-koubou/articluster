using System.IO;

using ArtiCluster.Applications.Services.Abstractions.Local.Strategies;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Applications.Services.Local.Strategies;

public sealed class LogicLocalOutputNamingStrategy
    : ILocalOutputNamingStrategy<UniversalDefinition>
{
    public string GetOutputDirectory( string baseDirectory, UniversalDefinition source )
        => Path.Combine( baseDirectory, "Logic", source.ManufacturerName.Value, source.ProductName.Value );

    public string GetOutputFileName( UniversalDefinition source )
        => source.PatchName.Value + ".plist";
}
