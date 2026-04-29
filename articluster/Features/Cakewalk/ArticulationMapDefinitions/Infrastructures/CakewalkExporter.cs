using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.Gateways;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.Infrastructures;

public class CakewalkExporter : IDefinitionExporter
{
    public async Task<Result<Unit, ExportReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinitionProductSet source,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var mapResult = new CakewalkModelMapper().Map( source );

            if( mapResult.IsFailure )
            {
                return Result<Unit, ExportReason>.Failure( ExportReason.SerializationError );
            }

            var jsonText = JsonSerializer.Serialize( mapResult.Unwrap(), SerializationConstants.SerializerOptions );

            await writer.WriteAsync( jsonText, cancellationToken );

            return Result<Unit, ExportReason>.Success( Unit.Default );
        }
        catch( NotSupportedException e )
        {
            return Result<Unit, ExportReason>.Failure( ExportReason.SerializationError, e );
        }
        catch( IOException e )
        {
            return Result<Unit, ExportReason>.Failure( ExportReason.IoError, e );
        }
        catch( Exception e )
        {
            return Result<Unit, ExportReason>.Failure( ExportReason.OtherError, e );
        }
    }
}
