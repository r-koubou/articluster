using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Exports;
using ArtiCluster.Applications.Services.Abstractions.MarkdownExports.Builders;
using ArtiCluster.Applications.Services.Abstractions.MarkdownExports.Models;
using ArtiCluster.Applications.Services.Abstractions.MarkdownExports.Strategies;
using ArtiCluster.Commons;
using ArtiCluster.Shared.IO.Local;

namespace ArtiCluster.Applications.Services.Local.MarkdownExports.Strategies;

public sealed class ManufacturerIndexDocumentLayoutStrategy : IMarkdownDocumentLayoutStrategy
{
    private readonly IMarkdownExportIndexBuilder builder;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ManufacturerIndexDocumentLayoutStrategy( IMarkdownExportIndexBuilder builder )
    {
        this.builder = builder;
    }

    public bool CanHandle( string dawName )
        => dawName == "Cakewalk";

    public async Task<Result<Unit, ExportFailureReason>> ExportAsync(
        string markdownContentRootDirectory,
        string dawName,
        string manufacturerName,
        IReadOnlyCollection<ExportedFileEntry> entries,
        CancellationToken cancellationToken = default )
    {
        try
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
