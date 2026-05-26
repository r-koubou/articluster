using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.Abstractions.MarkdownExports.Builders;
using ArtiCluster.Applications.Services.Abstractions.MarkdownExports.Strategies;
using ArtiCluster.Applications.Services.Abstractions.Models;
using ArtiCluster.Commons;
using ArtiCluster.Shared.IO.Local;

namespace ArtiCluster.Applications.Services.Local.MarkdownExports.Strategies;

public sealed class ProductsListDocumentLayoutStrategy : IMarkdownDocumentLayoutStrategy
{
    private readonly IMarkdownExportIndexBuilder builder;
    private readonly IMarkdownProductIndexBuilder productIndexBuilder;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ProductsListDocumentLayoutStrategy(
        IMarkdownExportIndexBuilder builder,
        IMarkdownProductIndexBuilder productIndexBuilder )
    {
        this.builder             = builder;
        this.productIndexBuilder = productIndexBuilder;
    }

    public bool CanHandle( string dawName )
        => dawName != "Cakewalk";

    public async Task<Result<Unit, ExportFailureReason>> ExportAsync(
        string markdownContentRootDirectory,
        string dawName,
        string manufacturerName,
        IReadOnlyCollection<ExportedFileEntry> entries,
        CancellationToken cancellationToken = default )
    {
        try
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
                        GroupName = x.SubGroupName,
                        SubGroupName = null
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

            return Result<Unit, ExportFailureReason>.Success( Unit.Default );
        }
        catch( IOException e )
        {
            return Result<Unit, ExportFailureReason>.Failure( ExportFailureReason.IoError, e );
        }
        catch( Exception e )
        {
            return Result<Unit, ExportFailureReason>.Failure( ExportFailureReason.OtherError, e );
        }
    }
}
