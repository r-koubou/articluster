using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.LocalFileConversions.Strategies;
using ArtiCluster.Commons;
using ArtiCluster.Shared.IO.Local;

using Microsoft.Extensions.Logging;

namespace ArtiCluster.Applications.Services.LocalFileConversions.Executors;

public sealed partial class LocalFileExportExecutor<TSource> : ILocalFileExportExecutor<TSource>
{
    private readonly ILogger<LocalFileExportExecutor<TSource>> logger;

    // ReSharper disable once ConvertToPrimaryConstructor
    public LocalFileExportExecutor( ILoggerFactory loggerFactory )
    {
        logger = loggerFactory.CreateLogger<LocalFileExportExecutor<TSource>>();
    }

    public async Task<Result<Unit, ExportFailureReason>> ExecuteAsync(
        string baseOutputDirectory,
        IEnumerable<TSource> sources,
        ILocalFileExportStrategy<TSource> exportStrategy,
        CancellationToken cancellationToken = default )
    {
        foreach( var x in sources )
        {
            var outputDirectory = exportStrategy.GetOutputDirectory( baseOutputDirectory, x );
            var outputPath = Path.Combine( outputDirectory, exportStrategy.GetExportFileName( x ) );

            LogExportingToOutputPath( outputPath );

            try
            {
                Directory.CreateDirectory( outputDirectory );

                await using var writer = new LocalTextContentWriter( outputPath );
                var result = await exportStrategy.ExportAsync( writer, x, cancellationToken );

                if( result.IsFailure )
                {
                    return result;
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

    [LoggerMessage( LogLevel.Debug, "Exporting to {OutputPath}" )]
    partial void LogExportingToOutputPath( string outputPath );
}
