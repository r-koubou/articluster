using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Applications.Services.Abstractions;

public interface IUniversalDefinitionLocalFileService
{
    Task<Result<UniversalDefinitionProductCollection, ImportFailureReason>> ImportAsync(
        string definitionsDirectory,
        CancellationToken cancellationToken = default );

    Task<Result<Unit, ExportFailureReason>> ExportAsync(
        string outputPath,
        UniversalDefinition definition,
        CancellationToken cancellationToken = default );

    Task<Result<Unit, ExportFailureReason>> ExportTemplateAsync(
        string outputPath,
        CancellationToken cancellationToken = default );
}
