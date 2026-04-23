using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.Articulation.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.UniversalDefinition.Gateways;

public enum ImporterReason
{
    Ok,
    DeserializationError,
    IoError,
    OtherError
}

public interface IDefinitionImporter
{
    Task<Result<Articulation, ImporterReason>> ImportAsync(
        ITextContentReader reader,
        CancellationToken cancellationToken = default
    );
}
