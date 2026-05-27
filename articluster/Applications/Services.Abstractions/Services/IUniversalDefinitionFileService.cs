using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Exports;
using ArtiCluster.Applications.Services.Abstractions.Imports;
using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Applications.Services.Abstractions.Services;

public interface IUniversalDefinitionFileService
{
    Task<Result<IReadOnlyCollection<UniversalDefinition>, ImportFailureReason>> ImportAsync(
        string definitionsDirectory,
        CancellationToken cancellationToken = default );

    Task<Result<Unit, ExportFailureReason>> ExportAsync(
        string outputDirectory,
        UniversalDefinition definition,
        CancellationToken cancellationToken = default );

    Task<Result<Unit, ExportFailureReason>> ExportTemplateAsync(
        string outputDirectory,
        string patchName,
        CancellationToken cancellationToken = default );
}
