using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinitions.Gateways;
using ArtiCluster.Shared.Domain.Articulation.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.UniversalDefinitions.Facades;

public interface IUniversalDefinitionFacade
{
    public Task<Result<Articulation, ImportReason>> ImportAsync( ITextContentReader reader, CancellationToken cancellationToken = default );
    public Task<Result<Unit, ExportReason>> ExportAsync( ITextContentWriter writer, Articulation source, CancellationToken cancellationToken = default );
}
