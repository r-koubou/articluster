using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.StudioOne.Gateways;

public enum ExportReason
{
    SerializationError,
    IoError,
    OtherError
}

public interface IDefinitionExporter
{
    Task<Result<Unit, ExportReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinitionProductSet source,
        CancellationToken cancellationToken = default
    );
}
