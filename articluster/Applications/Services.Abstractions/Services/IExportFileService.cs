using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Exports;
using ArtiCluster.Applications.Services.Abstractions.MarkdownExports.Models;
using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Applications.Services.Abstractions.Services;

public interface IExportFileService
{
    string TargetDawName { get; }

    Task<Result<IReadOnlyCollection<ExportedFileEntry>, ExportFailureReason>> ExportAsync(
        string convertedOutputDirectory,
        IReadOnlyCollection<UniversalDefinition> definitions,
        CancellationToken cancellationToken = default );
}
