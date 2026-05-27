using ArtiCluster.Applications.Services.Abstractions.Imports.Models;

namespace ArtiCluster.Applications.Services.Abstractions.Imports.Strategies;

public interface IImportedFileEntryFactory<in TSource>
{
    ImportedFileEntry Create( string inputFileName, TSource source );
}
