using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.Abstractions.Models;
using ArtiCluster.Applications.Services.Abstractions.Strategies;
using ArtiCluster.Commons;
using ArtiCluster.Shared.IO.Local;

namespace ArtiCluster.Applications.Services.Local;

public sealed class MarkdownExportIndexFileService
{
    private readonly IMarkdownExportIndexBuilder builder;

    // ReSharper disable once ConvertToPrimaryConstructor
    public MarkdownExportIndexFileService( IMarkdownExportIndexBuilder builder )
    {
        this.builder = builder;
    }

    public async Task<Result<Unit, ExportFailureReason>> ExportAsync(
        string markdownContentRootDirectory,
        IReadOnlyCollection<ExportedFileEntry> entries,
        CancellationToken cancellationToken = default )
    {
        var groups = entries
           .GroupBy( x => new { x.DawName, x.ManufacturerName } );

        foreach( var group in groups )
        {
            var markdownOutputDirectory = Path.Combine(
                markdownContentRootDirectory,
                group.Key.DawName,
                group.Key.ManufacturerName
            );

            var markdownOutputPath = Path.Combine(
                markdownOutputDirectory,
                "index.md"
            );

            Directory.CreateDirectory( markdownOutputDirectory );

            var markdown = builder.Build(
                group.ToArray(),
                markdownOutputDirectory
            );

            await using var writer = new LocalTextContentWriter( markdownOutputPath );
            await writer.WriteAsync( markdown, cancellationToken );
        }

        return Result<Unit, ExportFailureReason>.Success( Unit.Default );
    }
}
