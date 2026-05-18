using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.Abstractions.Collectors;
using ArtiCluster.Applications.Services.Abstractions.Models;
using ArtiCluster.Applications.Services.Abstractions.Services;
using ArtiCluster.Applications.Services.Local.Runners;
using ArtiCluster.Applications.Services.Local.Strategies;
using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

using Microsoft.Extensions.Logging;

namespace ArtiCluster.Applications.Services.Local;

public sealed class StudioOneExportFileService : IExportFileService
{
    private readonly ILoggerFactory loggerFactory;

    public string TargetDawName
        => "Studio One";

    // ReSharper disable once ConvertToPrimaryConstructor
    public StudioOneExportFileService( ILoggerFactory loggerFactory )
    {
        this.loggerFactory = loggerFactory;
    }

    public async Task<Result<IReadOnlyCollection<ExportedFileEntry>, ExportFailureReason>> ExportAsync(
        string convertedOutputDirectory,
        IReadOnlyCollection<UniversalDefinition> definitions,
        CancellationToken cancellationToken = default )
    {
        var runner = new FileExportRunner( loggerFactory );
        var namingStrategy = new StudioOneExportNamingStrategy();
        var strategy = new StudioOneFileExportStrategy();
        var factory = new StudioOneExportedFileEntryFactory();
        var collector = new InMemoryExportedFileCollector();

        var result = await runner.RunAsync(
            convertedOutputDirectory,
            definitions,
            namingStrategy,
            strategy,
            factory,
            collector,
            cancellationToken
        );

        return result.IsFailure
            ? Result<IReadOnlyCollection<ExportedFileEntry>, ExportFailureReason>.Failure( result.Reason )
            : Result<IReadOnlyCollection<ExportedFileEntry>, ExportFailureReason>.Success( collector.Items );

        // var markdownService = new MarkdownExportIndexFileService( new MarkdownExportIndexBuilder() );
        //
        // return await markdownService.ExportAsync(
        //     markdownContentOutputDirectory,
        //     collector.Items,
        //     cancellationToken
        // );
    }
}
