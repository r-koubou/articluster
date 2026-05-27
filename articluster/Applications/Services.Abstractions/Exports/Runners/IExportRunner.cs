using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Exports.Collectors;
using ArtiCluster.Applications.Services.Abstractions.Exports.Strategies;
using ArtiCluster.Commons;

namespace ArtiCluster.Applications.Services.Abstractions.Exports.Runners;

public interface IExportRunner
{
    Task<Result<Unit, ExportFailureReason>> RunAsync<TSource>(
        string outputBaseDirectory,
        IEnumerable<TSource> sources,
        IExportNamingStrategy<TSource> namingStrategy,
        IExportStrategy<TSource> strategy,
        IExportedFileEntryFactory<TSource>? entryFactory = null,
        IExportedFileCollector? collector = null,
        CancellationToken cancellationToken = default );
}
