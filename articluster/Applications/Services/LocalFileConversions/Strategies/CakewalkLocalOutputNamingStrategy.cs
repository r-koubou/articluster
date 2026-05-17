using System.IO;

using ArtiCluster.Applications.Services.Abstractions.Strategies;
using ArtiCluster.Shared.Domain.UniversalDefinitions;

namespace ArtiCluster.Applications.Services.LocalFileConversions.Strategies;

public sealed class CakewalkLocalOutputNamingStrategy
    : ILocalOutputNamingStrategy<ProductSet>
{
    public string GetOutputDirectory( string baseDirectory, ProductSet source )
        => Path.Combine( baseDirectory, "Cakewalk", source.ManufacturerName.Value );

    public string GetOutputFileName( ProductSet source )
        => source.ProductName.Value + ".artmap";
}
