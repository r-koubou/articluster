using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.UniversalDefinitions.Facades;

public enum ImportReason
{
    DeserializationError,
    IoError,
    OtherError
}

public enum ExportReason
{
    SerializationError,
    IoError,
    OtherError
}

public interface IUniversalDefinitionFacade
{
    public Task<Result<UniversalDefinition, ImportReason>> ImportAsync( ITextContentReader reader, CancellationToken cancellationToken = default );
    public Task<Result<Unit, ExportReason>> ExportAsync( ITextContentWriter writer, UniversalDefinition source, CancellationToken cancellationToken = default );
}
