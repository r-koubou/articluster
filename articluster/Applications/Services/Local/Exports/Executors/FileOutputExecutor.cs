using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Exports;
using ArtiCluster.Applications.Services.Abstractions.Exports.Collectors;
using ArtiCluster.Applications.Services.Abstractions.Exports.Executors;
using ArtiCluster.Applications.Services.Abstractions.Exports.Strategies;
using ArtiCluster.Commons;
using ArtiCluster.Shared.IO.Local;

using Microsoft.Extensions.Logging;

namespace ArtiCluster.Applications.Services.Local.Exports.Executors;

public sealed partial class FileOutputExecutor<TSource> : IExportExecutor<TSource>
{
    private readonly ILogger<FileOutputExecutor<TSource>> logger;

    // ReSharper disable once ConvertToPrimaryConstructor
    public FileOutputExecutor( ILoggerFactory loggerFactory )
    {
        logger = loggerFactory.CreateLogger<FileOutputExecutor<TSource>>();
    }

    public async Task<Result<Unit, ExportFailureReason>> ExecuteAsync(
        string baseOutputDirectory,
        IEnumerable<TSource> sources,
        IExportNamingStrategy<TSource> exportNamingStrategy,
        IExportStrategy<TSource> exportStrategy,
        IExportedFileEntryFactory<TSource>? entryFactory = null,
        IExportedFileCollector? collector = null,
        CancellationToken cancellationToken = default )
    {
        foreach( var x in sources )
        {
            var outputDirectory = exportNamingStrategy.GetOutputDirectory( baseOutputDirectory, x );
            var outputFileName = exportNamingStrategy.GetOutputFileName( x );
            var outputPath = Path.Combine( outputDirectory, outputFileName );

            LogExportingToOutputPath( outputPath );

            try
            {
                Directory.CreateDirectory( outputDirectory );

                await using var writer = new LocalTextContentWriter( outputPath );
                var result = await exportStrategy.ExportAsync( writer, x, cancellationToken );

                if( !result.IsSuccess )
                {
                    LogFailedToExportToOutput( outputPath, result.Reason, result.UnwrapError().Error );
                    return result;
                }

                // ReSharper disable once InvertIf
                if( entryFactory is not null && collector is not null )
                {
                    var entry = entryFactory.Create( outputDirectory, outputFileName, x );
                    await collector.CollectAsync( entry, cancellationToken );
                }

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

        return Result<Unit, ExportFailureReason>.Success( Unit.Default );
    }

    #region Logging
    [LoggerMessage( LogLevel.Debug, "Exporting to {OutputPath}" )]
    partial void LogExportingToOutputPath( string outputPath );

    [LoggerMessage( LogLevel.Error, "Failed to export to {OutputPath} with reason {Reason}" )]
    partial void LogFailedToExportToOutput( string outputPath, ExportFailureReason reason, Exception? exception );
    #endregion
}
