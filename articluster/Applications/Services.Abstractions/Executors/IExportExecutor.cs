using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Strategies;
using ArtiCluster.Commons;

namespace ArtiCluster.Applications.Services.Abstractions.Executors;

public interface IExportExecutor<TSource>
{
    Task<Result<Unit, ExportFailureReason>> ExecuteAsync(
        string baseOutputDirectory,
        IEnumerable<TSource> sources,
        IExportNamingStrategy<TSource> exportNamingStrategy,
        IExportStrategy<TSource> exportStrategy,
        CancellationToken cancellationToken = default
    );
}
