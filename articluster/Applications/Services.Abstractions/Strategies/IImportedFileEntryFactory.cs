using ArtiCluster.Applications.Services.Abstractions.Models;

namespace ArtiCluster.Applications.Services.Abstractions.Strategies;

public interface IImportedFileEntryFactory<in TSource>
{
    ImportedFileEntry Create( string inputFileName, TSource source );
}
