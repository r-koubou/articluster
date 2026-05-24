using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;

namespace ArtiCluster.Applications.Services.Abstractions.Executors;

public interface IImportExecutor<TTarget>
{
    Task<Result<TTarget, ImportFailureReason>> ExecuteAsync(
        string baseInputDirectory,
        CancellationToken cancellationToken = default
    );
}
