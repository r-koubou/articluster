using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.Abstractions.Collectors;
using ArtiCluster.Applications.Services.Abstractions.Runners;
using ArtiCluster.Applications.Services.Abstractions.Strategies;
using ArtiCluster.Applications.Services.Local.Executors;
using ArtiCluster.Commons;

using Microsoft.Extensions.Logging;

namespace ArtiCluster.Applications.Services.Local.Runners;

public sealed class FileImportRunner : IImportRunner
{
    private readonly ILoggerFactory loggerFactory;

    // ReSharper disable once ConvertToPrimaryConstructor
    public FileImportRunner( ILoggerFactory loggerFactory )
    {
        this.loggerFactory = loggerFactory;
    }

    public async Task<Result<Unit, ImportFailureReason>> RunAsync<TTarget>(
        string inputDirectory,
        IImportStrategy<TTarget> strategy,
        IImportNamingStrategy namingStrategy,
        IImportedFileEntryFactory<TTarget>? entryFactory = null,
        IImportedFileCollector? collector = null,
        CancellationToken cancellationToken = default )
    {
        var executor = new FileInputExecutor<TTarget>( loggerFactory );

        var result = await executor.ExecuteAsync(
            inputDirectory,
            strategy,
            namingStrategy,
            entryFactory,
            collector,
            cancellationToken
        );

        if( result.IsFailure )
        {
            return result.MapError( reason => reason switch
                {
                    ImportFailureReason.DeserializationError => ImportFailureReason.DeserializationError,
                    ImportFailureReason.IoError              => ImportFailureReason.IoError,
                    _                                        => ImportFailureReason.OtherError
                }
            );
        }

        return Result<Unit, ImportFailureReason>.Success( Unit.Default );
    }
}
