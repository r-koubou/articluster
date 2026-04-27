using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions;

namespace ArtiCluster.Applications.Services.Abstractions;

public enum BulkImportReason
{
    DeserializationError,
    IoError,
    OtherError
}

public interface IBulkImportUniversalDefinitionService
{
    Task<Result<UniversalDefinitionProductCollection, BulkImportReason>> ImportAsync( string definitionsDirectory, CancellationToken cancellationToken = default );
}
