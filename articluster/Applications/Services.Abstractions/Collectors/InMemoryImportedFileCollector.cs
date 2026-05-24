using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Models;

namespace ArtiCluster.Applications.Services.Abstractions.Collectors;

public sealed class InMemoryImportedFileCollector : IImportedFileCollector
{
    private readonly List<ImportedFileEntry> items = [ ];

    public IReadOnlyList<ImportedFileEntry> Items
        => items;

    public Task CollectAsync(
        ImportedFileEntry entry,
        CancellationToken cancellationToken = default )
    {
        items.Add( entry );
        return Task.CompletedTask;
    }
}
