using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.UniversalDefinitions.Gateways;

public enum ImportReason
{
    DeserializationError,
    IoError,
    OtherError
}

public interface IDefinitionImporter
{
    Task<Result<UniversalDefinition, ImportReason>> ImportAsync(
        ITextContentReader reader,
        CancellationToken cancellationToken = default
    );
}
