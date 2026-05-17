using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Local.Strategies;
using ArtiCluster.Commons;

namespace ArtiCluster.Applications.Services.Abstractions.Local.Executors;

public interface ILocalFileExportExecutor<TSource>
{
    Task<Result<Unit, ExportFailureReason>> ExecuteAsync(
        string baseOutputDirectory,
        IEnumerable<TSource> sources,
        ILocalOutputNamingStrategy<TSource> outputNamingStrategy,
        ILocalFileExportStrategy<TSource> exportStrategy,
        CancellationToken cancellationToken = default
    );
}
