using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.LocalFileExporting;
using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions;

namespace ArtiCluster.Applications.Services;

public sealed class CakewalkLocalFileConvertingService : ILocalFileConvertingService
{
    public string TargetDawName
        => "Cakewalk";

    public async Task<Result<Unit, ConvertReason>> ConvertAsync( string outputBaseDirectory, UniversalDefinitionProductCollection definitions, CancellationToken cancellationToken = default )
    {
        var executor = new LocalFileExportExecutor<UniversalDefinitionProductSet>();
        var strategy = new CakewalkLocalFileExportStrategy();

        // Export
        foreach( var x in definitions.Items )
        {
            var result = await executor.ExecuteAsync( outputBaseDirectory, [ x ], strategy, cancellationToken );

            if( result.IsFailure )
            {
                return result.MapError( reason => reason switch
                    {
                        ExportFailureReason.SerializationError => ConvertReason.SerializationError,
                        ExportFailureReason.IoError            => ConvertReason.IoError,
                        _                                      => ConvertReason.OtherError
                    }
                );
            }
        }

        return Result<Unit, ConvertReason>.Success( Unit.Default );
    }
}
