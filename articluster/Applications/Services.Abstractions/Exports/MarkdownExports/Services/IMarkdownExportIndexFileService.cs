using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Exports.Models;
using ArtiCluster.Commons;

namespace ArtiCluster.Applications.Services.Abstractions.Exports.MarkdownExports.Services;

public interface IMarkdownExportIndexFileService
{
    Task<Result<Unit, ExportFailureReason>> ExportAsync(
        string markdownContentRootDirectory,
        IReadOnlyCollection<ExportedFileEntry> entries,
        CancellationToken cancellationToken = default );
}
