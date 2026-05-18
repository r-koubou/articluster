using ArtiCluster.Applications.Services.Abstractions.Models;

namespace ArtiCluster.Applications.Services.Abstractions.Strategies;

public interface IExportedFileEntryFactory<in TSource>
{
    ExportedFileEntry Create( string outputDirectory, string outputFileName, TSource source );
}
