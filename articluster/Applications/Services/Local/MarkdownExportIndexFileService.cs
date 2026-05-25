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
           .GroupBy( x => new
                {
                    x.DawName,
                    x.ManufacturerName
                }
            );

        foreach( var group in groups )
        {
            // Cakewalk allows grouping all products in a single file, but other DAWs cannot do this.
            // Since it is necessary to group by product, branch as a special case.
            if( group.Key.DawName != "Cakewalk" )
            {
                await ExportManufacturerWithProductListAsync(
                    markdownContentRootDirectory,
                    group.Key.DawName,
                    group.Key.ManufacturerName,
                    group.ToArray(),
                    cancellationToken
                );

                continue;
            }

            await ExportDefaultManufacturerAsync(
                markdownContentRootDirectory,
                group.Key.DawName,
                group.Key.ManufacturerName,
                group.ToArray(),
                cancellationToken
            );
        }

        return Result<Unit, ExportFailureReason>.Success( Unit.Default );
    }

    private async Task ExportDefaultManufacturerAsync(
        string markdownContentRootDirectory,
        string dawName,
        string manufacturerName,
        IReadOnlyCollection<ExportedFileEntry> entries,
        CancellationToken cancellationToken )
    {
        var markdownOutputDirectory = Path.Combine(
            markdownContentRootDirectory,
            dawName,
            manufacturerName
        );

        var markdownOutputPath = Path.Combine(
            markdownOutputDirectory,
            "index.md"
        );

        Directory.CreateDirectory( markdownOutputDirectory );

        var markdown = builder.Build(
            manufacturerName,
            entries,
            markdownOutputDirectory
        );

        await using var writer = new LocalTextContentWriter( markdownOutputPath );
        await writer.WriteAsync( markdown, cancellationToken );
    }

    private async Task ExportManufacturerWithProductListAsync(
        string markdownContentRootDirectory,
        string dawName,
        string manufacturerName,
        IReadOnlyCollection<ExportedFileEntry> entries,
        CancellationToken cancellationToken )
    {
        var manufacturerDirectory = Path.Combine(
            markdownContentRootDirectory,
            dawName,
            manufacturerName
        );

        Directory.CreateDirectory( manufacturerDirectory );

        var productGroups = entries
                           .Where( x => !string.IsNullOrWhiteSpace( x.GroupName ) )
                           .GroupBy( x => x.GroupName! )
                           .OrderBy( x => x.Key )
                           .ToArray();

        var productIndexBuilder = new DefaultMarkdownProductIndexBuilder();

        // 1. index.md is a list of Product pages
        var manufacturerIndexMarkdown = productIndexBuilder.Build(
            manufacturerName,
            productGroups.Select( x => x.Key ).ToArray()
        );

        var manufacturerIndexPath = Path.Combine( manufacturerDirectory, "index.md" );

        await using( var writer = new LocalTextContentWriter( manufacturerIndexPath ) )
        {
            await writer.WriteAsync( manufacturerIndexMarkdown, cancellationToken );
        }

        // 2. Product detail pages
        foreach( var productGroup in productGroups )
        {
            var productName = productGroup.Key;

            var productMarkdownPath = Path.Combine(
                manufacturerDirectory,
                $"{productName}.md"
            );

            var productEntries = productGroup.Select( x => x with
                {
                    GroupName = null
                }
            ).ToArray();

            var productMarkdown = builder.Build(
                productName,
                productEntries,
                manufacturerDirectory
            );

            await using var writer = new LocalTextContentWriter( productMarkdownPath );
            await writer.WriteAsync( productMarkdown, cancellationToken );
        }
    }
}
