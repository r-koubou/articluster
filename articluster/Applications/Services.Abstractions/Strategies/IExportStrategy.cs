using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Applications.Services.Abstractions.Strategies;

public interface IExportStrategy<in TSource>
{
    Task<Result<Unit, ExportFailureReason>> ExportAsync(
        ITextContentWriter writer,
        TSource source,
        CancellationToken cancellationToken = default
    );
}
