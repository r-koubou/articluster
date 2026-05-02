using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.Logic.Articulations.Contracts;

public interface ILogicDefinitionFacade
{
    public Task<Result<Unit, ExportFailureReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinition source,
        CancellationToken cancellationToken = default );
}
