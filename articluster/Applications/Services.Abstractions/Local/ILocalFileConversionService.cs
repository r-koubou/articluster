using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Applications.Services.Abstractions.Local;

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
        IReadOnlyCollection<UniversalDefinition> definitions,
        CancellationToken cancellationToken = default
    );
}
