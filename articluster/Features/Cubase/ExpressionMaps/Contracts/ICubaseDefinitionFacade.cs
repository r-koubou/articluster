using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.Cubase.ExpressionMaps.Contracts;

public interface ICubaseDefinitionFacade
{
    public Task<Result<Unit, ExportFailureReason>> ExportAsync(
        ITextContentWriter writer,
        SeparatedArticulationGroupSet source,
        CancellationToken cancellationToken = default );
}
