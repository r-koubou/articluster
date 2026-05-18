using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Strategies;
using ArtiCluster.Commons;

namespace ArtiCluster.Applications.Services.Abstractions.Runners;

public interface IExportRunner
{
    Task<Result<Unit, ExportFailureReason>> RunAsync<TSource>(
        string outputBaseDirectory,
        IEnumerable<TSource> sources,
        IExportNamingStrategy<TSource> namingStrategy,
        IExportStrategy<TSource> strategy,
        CancellationToken cancellationToken = default );
}
