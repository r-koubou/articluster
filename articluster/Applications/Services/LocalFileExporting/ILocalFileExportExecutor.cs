using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;

namespace ArtiCluster.Applications.Services.LocalFileExporting;

public interface ILocalFileExportExecutor<TSource>
{
    Task<Result<Unit, ExportFailureReason>> ExecuteAsync(
        string baseOutputDirectory,
        IEnumerable<TSource> sources,
        ILocalFileExportStrategy<TSource> exportStrategy,
        CancellationToken cancellationToken = default
    );
}
