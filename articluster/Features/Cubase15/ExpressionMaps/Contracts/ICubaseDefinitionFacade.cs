using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.Cubase15.ExpressionMaps.Contracts;

public interface ICubaseDefinitionFacade<in TSource>
{
    public Task<Result<Unit, ExportFailureReason>> ExportAsync(
        ITextContentWriter writer,
        TSource source,
        CancellationToken cancellationToken = default );
}
