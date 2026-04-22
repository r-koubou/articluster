using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinition.Gateways;
using ArtiCluster.Shared.Domain.Articulation.Model;

namespace ArtiCluster.UniversalDefinition.UseCase.Abstraction;

public interface IExportUseCase
{
    Task<Result<Unit, Unit>> ExecuteAsync(
        IDefinitionExporter exporter,
        Articulation source,
        CancellationToken cancellationToken = default );
}
