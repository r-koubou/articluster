using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.LocalFileExporting;
using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Applications.Services;

public sealed class CubaseLocalFileConvertingService : ILocalFileConvertingService
{
    public string TargetDawName
        => "Cubase";

    public async Task<Result<Unit, ConvertReason>> ConvertAsync( string outputBaseDirectory, UniversalDefinitionProductCollection definitions, CancellationToken cancellationToken = default )
    {
        var executor = new LocalFileExportExecutor<UniversalDefinition>();
        var strategy = new CubaseLocalFileExportStrategy();

        foreach( var productSet in definitions.Items )
        {
            foreach( var definition in productSet.Items )
            {
                var result = await executor.ExecuteAsync( outputBaseDirectory, [ definition ], strategy, cancellationToken );

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
        }

        return Result<Unit, ConvertReason>.Success( Unit.Default );
    }
}
