using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.LocalFileConversions.Strategies;
using ArtiCluster.Commons;

namespace ArtiCluster.Applications.Services.LocalFileConversions.Executors;

public interface ILocalFileExportExecutor<TSource>
{
    Task<Result<Unit, ExportFailureReason>> ExecuteAsync(
        string baseOutputDirectory,
        IEnumerable<TSource> sources,
        ILocalFileExportStrategy<TSource> exportStrategy,
        CancellationToken cancellationToken = default
    );
}
