using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions;

namespace ArtiCluster.Applications.Services.Abstractions;

public enum ConvertFailureReason
{
    SerializationError,
    IoError,
    OtherError
}

public interface ILocalFileConversionService
{
    string TargetDawName { get; }

    Task<Result<Unit, ConvertFailureReason>> ConvertAsync(
        string outputBaseDirectory,
        UniversalDefinitionProductCollection definitions,
        CancellationToken cancellationToken = default
    );
}
