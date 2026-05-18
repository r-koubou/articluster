using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Models;

namespace ArtiCluster.Applications.Services.Abstractions.Collectors;

public interface IExportedFileCollector
{
    Task CollectAsync( ExportedFileEntry entry, CancellationToken cancellationToken = default );
}
