using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Applications.Services.Abstractions.Strategies;

public interface IImportStrategy<TTarget>
{
    Task<Result<TTarget, ImportFailureReason>> ImportAsync(
        ITextContentReader reader,
        CancellationToken cancellationToken = default
    );
}
