using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.Logic.Articulations.Contracts;

public interface ILogicDefinitionFacade
{
    public Task<Result<Unit, ExportFailureReason>> ExportAsync(
        ITextContentWriter writer,
        SeparatedArticulationGroupSet source,
        CancellationToken cancellationToken = default );
}
