using System.IO;

using ArtiCluster.Applications.Services.Abstractions.Exports.Strategies;
using ArtiCluster.Shared.Domain.UniversalDefinitions;

namespace ArtiCluster.Applications.Services.Local.Exports.Strategies;

public sealed class CubaseExportNamingStrategy
    : IExportNamingStrategy<SeparatedArticulationGroupSet>
{
    public string GetOutputDirectory( string baseDirectory, SeparatedArticulationGroupSet source )
        => Path.Combine( baseDirectory, "Cubase", source.ManufacturerName.Value, source.ProductName.Value, source.PatchName.Value );

    public string GetOutputFileName( SeparatedArticulationGroupSet source )
        => $"{source.ArticulationGroupName.Value}.expressionmap";
}
