using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Models;
using ArtiCluster.Commons;

namespace ArtiCluster.Applications.Services.Abstractions.MarkdownExports.Strategies;

public interface IMarkdownDocumentLayoutStrategy
{
    bool CanHandle( string dawName );

    Task<Result<Unit, ExportFailureReason>> ExportAsync(
        string markdownContentRootDirectory,
        string dawName,
        string manufacturerName,
        IReadOnlyCollection<ExportedFileEntry> entries,
        CancellationToken cancellationToken = default );
}
