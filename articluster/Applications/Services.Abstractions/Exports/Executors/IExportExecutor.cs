using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Exports.Collectors;
using ArtiCluster.Applications.Services.Abstractions.Exports.Strategies;
using ArtiCluster.Commons;

namespace ArtiCluster.Applications.Services.Abstractions.Exports.Executors;

public interface IExportExecutor<TSource>
{
    Task<Result<Unit, ExportFailureReason>> ExecuteAsync(
        string baseOutputDirectory,
        IEnumerable<TSource> sources,
        IExportNamingStrategy<TSource> exportNamingStrategy,
        IExportStrategy<TSource> exportStrategy,
        IExportedFileEntryFactory<TSource>? entryFactory = null,
        IExportedFileCollector? collector = null,
        CancellationToken cancellationToken = default
    );
}
