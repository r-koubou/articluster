using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.LocalFileConversions.Executors;
using ArtiCluster.Applications.Services.LocalFileConversions.Strategies;
using ArtiCluster.Commons;

namespace ArtiCluster.Applications.Services.LocalFileConversions.Runners;

public sealed class LocalFileConversionRunner : ILocalFileConversionRunner
{
    public async Task<Result<Unit, ConvertFailureReason>> RunAsync<TSource>(
        string outputBaseDirectory,
        IEnumerable<TSource> sources,
        ILocalFileExportStrategy<TSource> strategy,
        CancellationToken cancellationToken = default )
    {
        var executor = new LocalFileExportExecutor<TSource>();

        foreach( var x in sources )
        {
            var result = await executor.ExecuteAsync( outputBaseDirectory, [ x ], strategy, cancellationToken );

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
