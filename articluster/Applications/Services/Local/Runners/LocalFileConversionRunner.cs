using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.Abstractions.Local;
using ArtiCluster.Applications.Services.Abstractions.Local.Runners;
using ArtiCluster.Applications.Services.Abstractions.Local.Strategies;
using ArtiCluster.Applications.Services.Local.Executors;
using ArtiCluster.Commons;

using Microsoft.Extensions.Logging;

namespace ArtiCluster.Applications.Services.Local.Runners;

public sealed class LocalFileConversionRunner : ILocalFileConversionRunner
{
    private readonly ILoggerFactory loggerFactory;

    // ReSharper disable once ConvertToPrimaryConstructor
    public LocalFileConversionRunner( ILoggerFactory loggerFactory )
    {
        this.loggerFactory = loggerFactory;
    }

    public async Task<Result<Unit, ConvertFailureReason>> RunAsync<TSource>(
        string outputBaseDirectory,
        IEnumerable<TSource> sources,
        ILocalOutputNamingStrategy<TSource> namingStrategy,
        ILocalFileExportStrategy<TSource> strategy,
        CancellationToken cancellationToken = default )
    {
        var executor = new LocalFileExportExecutor<TSource>( loggerFactory );

        foreach( var x in sources )
        {
            var result = await executor.ExecuteAsync(
                outputBaseDirectory,
                [ x ],
                namingStrategy,
                strategy,
                cancellationToken
            );

            if( result.IsFailure )
            {
                return result.MapError( reason => reason switch
                    {
                        ExportFailureReason.SerializationError => ConvertFailureReason.SerializationError,
                        ExportFailureReason.IoError            => ConvertFailureReason.IoError,
                        _                                      => ConvertFailureReason.OtherError
                    }
                );
            }
        }

        return Result<Unit, ConvertFailureReason>.Success( Unit.Default );
    }
}
