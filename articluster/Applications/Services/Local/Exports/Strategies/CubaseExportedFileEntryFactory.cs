using ArtiCluster.Applications.Services.Abstractions.Exports.Strategies;
using ArtiCluster.Applications.Services.Abstractions.MarkdownExports.Models;
using ArtiCluster.Shared.Domain.UniversalDefinitions;

namespace ArtiCluster.Applications.Services.Local.Exports.Strategies;

public sealed class CubaseExportedFileEntryFactory
    : IExportedFileEntryFactory<SeparatedArticulationGroupSet>
{
    public ExportedFileEntry Create( string outputDirectory, string outputFileName, SeparatedArticulationGroupSet source )
    {
        return new ExportedFileEntry(
            DawName: "Cubase",
            ManufacturerName: source.ManufacturerName.Value,
            OutputDirectory: outputDirectory,
            OutputFileName: outputFileName,
            DisplayName:source.ArticulationGroupName.Value,
            GroupName: source.ProductName.Value,
            SubGroupName: source.PatchName.Value
        );
    }
}
