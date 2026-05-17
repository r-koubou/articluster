using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Applications.Services.Abstractions;

public enum GenerateFailureReason
{
    IoError,
    OtherError
}

public interface ILocalFileMarkdownGenerationService
{
    string TargetDawName { get; }

    Task<Result<Unit, ConvertFailureReason>> GenerateAsync(
        string outputBaseDirectory,
        IReadOnlyCollection<UniversalDefinition> definitions,
        CancellationToken cancellationToken = default
    );
}
