using ArtiCluster.Applications.Services.Abstractions.Models;
using ArtiCluster.Applications.Services.Abstractions.Strategies;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Applications.Services.Local.Strategies;

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
