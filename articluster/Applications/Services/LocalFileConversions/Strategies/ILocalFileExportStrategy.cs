using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Applications.Services.LocalFileConversions.Strategies;

public interface ILocalFileExportStrategy<in TSource>
{
    string GetOutputDirectory( string baseDirectory, TSource source );

    string GetExportFileName( TSource source );

    Task<Result<Unit, ExportFailureReason>> ExportAsync(
        ITextContentWriter writer,
        TSource source,
        CancellationToken cancellationToken = default
    );
}
