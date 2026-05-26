using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.MarkdownExports.Models;

namespace ArtiCluster.Applications.Services.Abstractions.Exports.Collectors;

public interface IExportedFileCollector
{
    Task CollectAsync( ExportedFileEntry entry, CancellationToken cancellationToken = default );
}
