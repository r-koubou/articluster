using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions;

namespace ArtiCluster.Applications.Services.Abstractions;

public enum ConvertReason
{
    SerializationError,
    IoError,
    OtherError
}

public interface ILocalFileConversionService
{
    string TargetDawName { get; }

    Task<Result<Unit, ConvertReason>> ConvertAsync(
        string outputBaseDirectory,
        UniversalDefinitionProductCollection definitions,
        CancellationToken cancellationToken = default
    );
}
