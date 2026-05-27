using ArtiCluster.Applications.Services.Abstractions.Imports.Models;
using ArtiCluster.Applications.Services.Abstractions.Imports.Strategies;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Applications.Services.Local.Imports.Strategies;

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
