using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Imports.Collectors;
using ArtiCluster.Applications.Services.Abstractions.Imports.Strategies;
using ArtiCluster.Commons;

namespace ArtiCluster.Applications.Services.Abstractions.Imports.Runners;

public interface IImportRunner
{
    Task<Result<Unit, ImportFailureReason>> RunAsync<TSource>(
        string inputDirectory,
        IImportStrategy<TSource> strategy,
        IImportNamingStrategy namingStrategy,
        IImportedFileEntryFactory<TSource>? entryFactory = null,
        IImportedFileCollector? collector = null,
        CancellationToken cancellationToken = default );
}
