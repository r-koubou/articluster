using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.Cubase.ExpressionMapDefinitions.Infrastructures;
using ArtiCluster.Features.Cubase.ExpressionMapDefinitions.UseCases;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;
using ArtiCluster.Shared.IO.Local;

using GatewayReason = ArtiCluster.Features.Cubase.ExpressionMapDefinitions.Gateways.ExportReason;

namespace ArtiCluster.Features.Cubase.ExpressionMapDefinitions.Facades;

public sealed class CubaseDefinitionFacade : ICubaseDefinitionFacade
{
    public async Task<Result<Unit, ExportReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinition source,
        CancellationToken cancellationToken = default )
    {
        var exporter = new CubaseExporter();
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

    public async Task<Result<Unit, ExportReason>> ExportAsync( string exportDirectory, UniversalDefinition source, CancellationToken cancellationToken = default )
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

        var outputPath = Path.Combine( outputDirectory, $"{source.ProductName.Value}.expressionmap" );

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
    public static string BuildExportDirectory( string baseDirectory, UniversalDefinition source )
    {
        return Path.Combine(
            baseDirectory,
            source.ManufacturerName.Value,
            source.ProductName.Value
        );
    }
}
