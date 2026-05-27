using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Imports.Collectors;
using ArtiCluster.Applications.Services.Abstractions.Imports.Strategies;
using ArtiCluster.Commons;

namespace ArtiCluster.Applications.Services.Abstractions.Imports.Executors;

public interface IImportExecutor<TTarget>
{
    Task<Result<Unit, ImportFailureReason>> ExecuteAsync(
        string inputDirectory,
        IImportStrategy<TTarget> strategy,
        IImportNamingStrategy namingStrategy,
        IImportedFileEntryFactory<TTarget>? entryFactory,
        IImportedFileCollector? collector,
        CancellationToken cancellationToken = default
    );
}
