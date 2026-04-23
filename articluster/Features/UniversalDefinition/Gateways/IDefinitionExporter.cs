using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.Articulation.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.UniversalDefinition.Gateways;

public enum ExportReason
{
    Ok,
    SerializationError,
    IoError,
    OtherError
}

public interface IDefinitionExporter
{
    Task<Result<Unit, ExportReason>> ExportAsync(
        ITextContentWriter writer,
        Articulation source,
        CancellationToken cancellationToken = default
    );
}
