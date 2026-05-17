using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions;

namespace ArtiCluster.Applications.Services.Abstractions;

public interface ILocalBulkImportUniversalDefinitionService
{
    Task<Result<ProductCollection, ImportFailureReason>> ImportAsync(
        string definitionsDirectory,
        CancellationToken cancellationToken = default );
}
