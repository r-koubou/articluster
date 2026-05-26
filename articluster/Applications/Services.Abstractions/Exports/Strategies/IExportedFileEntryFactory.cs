using ArtiCluster.Applications.Services.Abstractions.MarkdownExports.Models;

namespace ArtiCluster.Applications.Services.Abstractions.Exports.Strategies;

public interface IExportedFileEntryFactory<in TSource>
{
    ExportedFileEntry Create( string outputDirectory, string outputFileName, TSource source );
}
