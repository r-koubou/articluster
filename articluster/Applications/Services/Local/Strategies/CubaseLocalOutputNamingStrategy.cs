using System.IO;

using ArtiCluster.Applications.Services.Abstractions.Local.Strategies;
using ArtiCluster.Shared.Domain.UniversalDefinitions;

namespace ArtiCluster.Applications.Services.Local.Strategies;

public sealed class CubaseLocalOutputNamingStrategy
    : ILocalOutputNamingStrategy<SeparatedArticulationGroupSet>
{
    public string GetOutputDirectory( string baseDirectory, SeparatedArticulationGroupSet source )
        => Path.Combine( baseDirectory, "Cubase", source.ManufacturerName.Value, source.ProductName.Value, source.PatchName.Value );

    public string GetOutputFileName( SeparatedArticulationGroupSet source )
        => $"{source.ArticulationGroupName.Value}.expressionmap";
}
