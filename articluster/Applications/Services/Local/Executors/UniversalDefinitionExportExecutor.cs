using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.Abstractions.Collectors;
using ArtiCluster.Applications.Services.Abstractions.Executors;
using ArtiCluster.Applications.Services.Abstractions.Strategies;
using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Local;

using Microsoft.Extensions.Logging;

namespace ArtiCluster.Applications.Services.Local.Executors;

public sealed class UniversalDefinitionExportExecutor : IExportExecutor<UniversalDefinition>
{
    private readonly ILogger<UniversalDefinitionExportExecutor> logger;

    // ReSharper disable once ConvertToPrimaryConstructor
    public UniversalDefinitionExportExecutor( ILoggerFactory loggerFactory )
    {
        logger = loggerFactory.CreateLogger<UniversalDefinitionExportExecutor>();
    }

    public async Task<Result<Unit, ExportFailureReason>> ExecuteAsync(
        string baseOutputDirectory,
        IEnumerable<UniversalDefinition> sources,
        IExportNamingStrategy<UniversalDefinition> exportNamingStrategy,
        IExportStrategy<UniversalDefinition> exportStrategy,
        IExportedFileEntryFactory<UniversalDefinition>? entryFactory = null,
        IExportedFileCollector? collector = null,
        CancellationToken cancellationToken = default )
    {
        try
        {
            ArgumentNullException.ThrowIfNull( baseOutputDirectory );

            // outputPath has parent directory, create it if it doesn't exist
            if( baseOutputDirectory.Length > 0 )
            {
                Directory.CreateDirectory( baseOutputDirectory );
            }

            foreach( var definition in sources )
            {
                var outputDirectory = exportNamingStrategy.GetOutputDirectory( baseOutputDirectory, definition );
                var outputFileName = exportNamingStrategy.GetOutputFileName( definition );
                var outputPath = Path.Combine( outputDirectory, outputFileName );

                await using var writer = new LocalTextContentWriter( outputPath );

                var result = await exportStrategy.ExportAsync( writer, definition, cancellationToken );

                if( result.IsFailure )
                {
                    return result;
                }

                // ReSharper disable once InvertIf
                if( entryFactory != null && collector != null )
                {
                    var entry = entryFactory.Create( outputDirectory, outputDirectory, definition );
                    await collector.CollectAsync( entry, cancellationToken );
                }
            }

            return Result<Unit, ExportFailureReason>.Success( Unit.Default );
        }
        catch( IOException )
        {
            return Result<Unit, ExportFailureReason>.Failure( ExportFailureReason.IoError );
        }
        catch( Exception )
        {
            return Result<Unit, ExportFailureReason>.Failure( ExportFailureReason.OtherError );
        }
    }
}
