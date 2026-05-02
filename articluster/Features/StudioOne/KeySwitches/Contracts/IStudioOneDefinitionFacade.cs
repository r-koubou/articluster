using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.StudioOne.KeySwitches.Contracts;

public enum ExportFailureReason
{
    SerializationError,
    IoError,
    OtherError
}

public interface IStudioOneDefinitionFacade
{
    public Task<Result<Unit, ExportFailureReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinitionProductSet source,
        CancellationToken cancellationToken = default );
}
