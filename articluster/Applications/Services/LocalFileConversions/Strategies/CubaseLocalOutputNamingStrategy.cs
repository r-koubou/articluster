using System.IO;

using ArtiCluster.Applications.Services.Abstractions.Strategies;
using ArtiCluster.Shared.Domain.UniversalDefinitions;

namespace ArtiCluster.Applications.Services.LocalFileConversions.Strategies;

public sealed class CubaseLocalOutputNamingStrategy
    : ILocalOutputNamingStrategy<SeparatedArticulationGroupSet>
{
    public string GetOutputDirectory( string baseDirectory, SeparatedArticulationGroupSet source )
        => Path.Combine( baseDirectory, "Cubase", source.ManufacturerName.Value, source.ProductName.Value, source.PatchName.Value );

    public string GetOutputFileName( SeparatedArticulationGroupSet source )
        => $"{source.ArticulationGroupName.Value}.expressionmap";
}
