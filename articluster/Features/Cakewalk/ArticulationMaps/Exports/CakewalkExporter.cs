using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.Cakewalk.ArticulationMaps.Contracts;
using ArtiCluster.Features.Cakewalk.ArticulationMaps.Mappers;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.Cakewalk.ArticulationMaps.Exports;

public sealed class CakewalkExporter
{
    public async Task<Result<Unit, ExportFailureReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinitionProductSet source,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var mapResult = new CakewalkModelMapper().Map( source );

            if( mapResult.IsFailure )
            {
                return Result<Unit, ExportFailureReason>.Failure( ExportFailureReason.SerializationError );
            }

            var jsonText = JsonSerializer.Serialize( mapResult.Unwrap(), SerializationConstants.SerializerOptions );

            await writer.WriteAsync( jsonText, cancellationToken );

            return Result<Unit, ExportFailureReason>.Success( Unit.Default );
        }
        catch( NotSupportedException e )
        {
            return Result<Unit, ExportFailureReason>.Failure( ExportFailureReason.SerializationError, e );
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
}
