using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Exports;
using ArtiCluster.Applications.Services.Abstractions.Exports.MarkdownExports.Services;
using ArtiCluster.Applications.Services.Abstractions.Exports.MarkdownExports.Strategies;
using ArtiCluster.Applications.Services.Abstractions.Exports.Models;
using ArtiCluster.Commons;

namespace ArtiCluster.Applications.Services.Local.Exports.MarkdownExports.Services;

public sealed class MarkdownExportIndexFileService : IMarkdownExportIndexFileService
{
    private readonly IReadOnlyCollection<IMarkdownDocumentLayoutStrategy> strategies;

    // ReSharper disable once ConvertToPrimaryConstructor
    public MarkdownExportIndexFileService( IEnumerable<IMarkdownDocumentLayoutStrategy> strategies )
    {
        this.strategies = strategies.ToArray();
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
            var matchedStrategies =
                strategies.Where( x => x.CanHandle( group.Key.DawName ) ).ToArray();

            if( matchedStrategies.Length == 0 )
            {
                return Result<Unit, ExportFailureReason>.Failure(
                    ExportFailureReason.OtherError,
                    new KeyNotFoundException( $"No markdown layout strategy found for DAW: {group.Key.DawName}" )
                );
            }
            if( matchedStrategies.Length != 1 )
            {
                return Result<Unit, ExportFailureReason>.Failure(
                    ExportFailureReason.OtherError,
                    new InvalidOperationException( $"Multiple markdown layout strategies found for DAW: {group.Key.DawName}" )
                );
            }

            var strategy = matchedStrategies.First();

            var result = await strategy.ExportAsync(
                markdownContentRootDirectory,
                group.Key.DawName,
                group.Key.ManufacturerName,
                group.ToArray(),
                cancellationToken
            );

            if( result.IsFailure )
            {
                return result;
            }
        }

        return Result<Unit, ExportFailureReason>.Success( Unit.Default );
    }
}
