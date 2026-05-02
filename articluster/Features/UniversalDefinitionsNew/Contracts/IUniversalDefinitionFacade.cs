using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.UniversalDefinitionsNew.Contracts;

public interface IUniversalDefinitionFacade
{
    public Task<Result<UniversalDefinition, ImportFailureReason>> ImportAsync( ITextContentReader reader, CancellationToken cancellationToken = default );
    public Task<Result<Unit, ExportFailureReason>> ExportAsync( ITextContentWriter writer, UniversalDefinition source, CancellationToken cancellationToken = default );
}
