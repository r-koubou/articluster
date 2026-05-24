using ArtiCluster.Applications.Services.Abstractions.Models;
using ArtiCluster.Applications.Services.Abstractions.Strategies;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Applications.Services.Local.Strategies;

public sealed class UniversalDefinitionImportedFileEntryFactory
    : IImportedFileEntryFactory<UniversalDefinition>
{
    public ImportedFileEntry Create( string inputFileName, UniversalDefinition source )
    {
        return new ImportedFileEntry(
            FilePath: inputFileName,
            Definition: source
        );
    }
}
