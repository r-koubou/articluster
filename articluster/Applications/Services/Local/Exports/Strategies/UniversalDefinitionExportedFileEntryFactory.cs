using ArtiCluster.Applications.Services.Abstractions.Exports.Strategies;
using ArtiCluster.Applications.Services.Abstractions.MarkdownExports.Models;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Applications.Services.Local.Exports.Strategies;

public sealed class UniversalDefinitionExportedFileEntryFactory
    : IExportedFileEntryFactory<UniversalDefinition>
{
    public ExportedFileEntry Create( string outputDirectory, string outputFileName, UniversalDefinition source )
    {
        return new ExportedFileEntry(
            DawName: "Universal Definition",
            ManufacturerName: source.ManufacturerName.Value,
            OutputDirectory: outputDirectory,
            OutputFileName: outputFileName,
            DisplayName: $"{source.PatchName.Value}"
        );
    }
}
