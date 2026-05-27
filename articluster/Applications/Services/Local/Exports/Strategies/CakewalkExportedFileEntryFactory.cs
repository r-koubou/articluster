using ArtiCluster.Applications.Services.Abstractions.Exports.Models;
using ArtiCluster.Applications.Services.Abstractions.Exports.Strategies;
using ArtiCluster.Shared.Domain.UniversalDefinitions;

namespace ArtiCluster.Applications.Services.Local.Exports.Strategies;

public sealed class CakewalkExportedFileEntryFactory
    : IExportedFileEntryFactory<ProductSet>
{
    public ExportedFileEntry Create( string outputDirectory, string outputFileName, ProductSet source )
    {
        return new ExportedFileEntry(
            DawName: "Cakewalk",
            ManufacturerName: source.ManufacturerName.Value,
            OutputDirectory: outputDirectory,
            OutputFileName: outputFileName,
            DisplayName: source.ProductName.Value
        );
    }
}
