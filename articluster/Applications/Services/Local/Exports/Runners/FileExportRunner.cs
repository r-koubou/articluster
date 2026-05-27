using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Exports;
using ArtiCluster.Applications.Services.Abstractions.Exports.Collectors;
using ArtiCluster.Applications.Services.Abstractions.Exports.Runners;
using ArtiCluster.Applications.Services.Abstractions.Exports.Strategies;
using ArtiCluster.Applications.Services.Local.Exports.Executors;
using ArtiCluster.Commons;

using Microsoft.Extensions.Logging;

namespace ArtiCluster.Applications.Services.Local.Exports.Runners;

public sealed class FileExportRunner : IExportRunner
{
    private readonly ILoggerFactory loggerFactory;

    // ReSharper disable once ConvertToPrimaryConstructor
    public FileExportRunner( ILoggerFactory loggerFactory )
    {
        this.loggerFactory = loggerFactory;
    }

    public async Task<Result<Unit, ExportFailureReason>> RunAsync<TSource>(
        string outputBaseDirectory,
        IEnumerable<TSource> sources,
        IExportNamingStrategy<TSource> namingStrategy,
        IExportStrategy<TSource> strategy,
        IExportedFileEntryFactory<TSource>? entryFactory = null,
        IExportedFileCollector? collector = null,
        CancellationToken cancellationToken = default )
    {
        var executor = new FileOutputExecutor<TSource>( loggerFactory );

        foreach( var x in sources )
        {
            var result = await executor.ExecuteAsync(
                outputBaseDirectory,
                [ x ],
                namingStrategy,
                strategy,
                entryFactory,
                collector,
                cancellationToken
            );

            if( result.IsFailure )
            {
                return result.MapError( reason => reason switch
                    {
                        ExportFailureReason.SerializationError => ExportFailureReason.SerializationError,
                        ExportFailureReason.IoError            => ExportFailureReason.IoError,
                        _                                          => ExportFailureReason.OtherError
                    }
                );
            }
        }

        return Result<Unit, ExportFailureReason>.Success( Unit.Default );
    }
}
