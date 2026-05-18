using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Models;

namespace ArtiCluster.Applications.Services.Abstractions.Collectors;

public sealed class InMemoryExportedFileCollector : IExportedFileCollector
{
    private readonly List<ExportedFileEntry> items = [ ];

    public IReadOnlyList<ExportedFileEntry> Items
        => items;

    public Task CollectAsync(
        ExportedFileEntry entry,
        CancellationToken cancellationToken = default )
    {
        items.Add( entry );
        return Task.CompletedTask;
    }
}
