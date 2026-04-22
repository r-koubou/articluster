using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinition.Gateways;
using ArtiCluster.Shared.Domain.Articulation.Model;

namespace ArtiCluster.UniversalDefinition.UseCase.Abstraction;

public interface IImportUseCase
{
    Task<Result<Articulation, Unit>> ExecuteAsync(
        IDefinitionImporter importer,
        CancellationToken cancellationToken = default );
}