using System.IO;

using ArtiCluster.Applications.Services.Abstractions.Strategies;
using ArtiCluster.Shared.Domain.UniversalDefinitions;

namespace ArtiCluster.Applications.Services.Local.Strategies;

public sealed class CakewalkExportNamingStrategy
    : IExportNamingStrategy<ProductSet>
{
    public string GetOutputDirectory( string baseDirectory, ProductSet source )
        => Path.Combine( baseDirectory, "Cakewalk", source.ManufacturerName.Value );

    public string GetOutputFileName( ProductSet source )
        => source.ProductName.Value + ".artmap";
}
