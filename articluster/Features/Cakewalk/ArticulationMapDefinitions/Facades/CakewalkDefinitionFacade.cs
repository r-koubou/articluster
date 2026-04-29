using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.Infrastructures;
using ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.UseCases;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Abstractions;
using ArtiCluster.Shared.IO.Local;

using GatewayReason = ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.Gateways.ExportReason;

namespace ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.Facades;

public sealed class CakewalkDefinitionFacade : ICakewalkDefinitionFacade
{
    public async Task<Result<Unit, ExportReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinitionProductSet source,
        CancellationToken cancellationToken = default )
    {
        var exporter = new CakewalkExporter();
        var input = new ExportInputPort( source, exporter, writer );
        var useCase = new ExportUseCase();

        var result = await useCase.ExecuteAsync( input, cancellationToken );

        if( result.IsSuccess )
        {
            return Result<Unit, ExportReason>.Success( Unit.Default );
        }

        return result.MapError( reason =>
            {
                return reason switch
                {
                    GatewayReason.SerializationError => ExportReason.SerializationError,
                    GatewayReason.IoError            => ExportReason.IoError,
                    _                                => ExportReason.OtherError
                };
            }
        );
    }

    public async Task<Result<Unit, ExportReason>> ExportAsync( string exportDirectory, UniversalDefinitionProductSet source, CancellationToken cancellationToken = default )
    {
        var outputDirectory = BuildExportDirectory( exportDirectory, source );

        if( !Directory.Exists( outputDirectory ) )
        {
            try
            {
                Directory.CreateDirectory( outputDirectory );
            }
            catch( Exception e )
            {
                return Result<Unit, ExportReason>.Failure( ExportReason.IoError, e );
            }
        }

        var outputPath = Path.Combine( outputDirectory, $"{source.ProductName.Value}.artmap" );

        try
        {
            await using var writer = new LocalTextContentWriter( outputPath );

            return await ExportAsync( writer, source, cancellationToken );
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

    // ReSharper disable once MemberCanBePrivate.Global
    public static string BuildExportDirectory( string baseDirectory, UniversalDefinitionProductSet source )
    {
        return Path.Combine( baseDirectory, source.ManufacturerName.Value );
    }
}
