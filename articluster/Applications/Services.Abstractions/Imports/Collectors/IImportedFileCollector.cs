using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Imports.Models;

namespace ArtiCluster.Applications.Services.Abstractions.Imports.Collectors;

public interface IImportedFileCollector
{
    Task CollectAsync( ImportedFileEntry entry, CancellationToken cancellationToken = default );
}
