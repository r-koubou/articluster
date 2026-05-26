using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Exports;
using ArtiCluster.Applications.Services.Abstractions.Exports.Collectors;
using ArtiCluster.Applications.Services.Abstractions.MarkdownExports.Models;
using ArtiCluster.Applications.Services.Abstractions.Services;
using ArtiCluster.Applications.Services.Local.Exports.Runners;
using ArtiCluster.Applications.Services.Local.Exports.Strategies;
using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

using Microsoft.Extensions.Logging;

namespace ArtiCluster.Applications.Services.Local.Exports.Services;

public sealed class CubaseExportFileService : IExportFileService
{
    private readonly ILoggerFactory loggerFactory;

    public string TargetDawName
        => "Cubase";

    // ReSharper disable once ConvertToPrimaryConstructor
    public CubaseExportFileService( ILoggerFactory loggerFactory )
    {
        this.loggerFactory = loggerFactory;
    }

    public async Task<Result<IReadOnlyCollection<ExportedFileEntry>, ExportFailureReason>> ExportAsync(
        string convertedOutputDirectory,
        IReadOnlyCollection<UniversalDefinition> definitions,
        CancellationToken cancellationToken = default )
    {
        var runner = new FileExportRunner( loggerFactory );
        var namingStrategy = new CubaseExportNamingStrategy();
        var strategy = new CubaseFileExportStrategy();
        var factory = new CubaseExportedFileEntryFactory();
        var collector = new InMemoryExportedFileCollector();

        var collection = new SeparatedArticulationGroupCollection( definitions );

        var result = await runner.RunAsync(
            convertedOutputDirectory,
            collection.Items,
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
