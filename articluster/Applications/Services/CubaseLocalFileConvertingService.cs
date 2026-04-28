using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Commons;
using ArtiCluster.Features.Cubase.ExpressionMapDefinitions.Facades;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Local;

namespace ArtiCluster.Applications.Services;

public sealed class CubaseLocalFileConvertingService( string outputBaseDirectory ) : IConvertingService
{
    public string TargetDawName
        => "Cubase";

    public async Task<Result<Unit, ConvertReason>> ConvertAsync( UniversalDefinitionProductCollection definitions, CancellationToken cancellationToken = default )
    {
        foreach( var productSet in definitions.Items )
        {
            foreach( var definition in productSet.Items )
            {
                var result = await ConvertImplAsync( definition, cancellationToken );

                if( result.IsFailure )
                {
                    return result;
                }
            }
        }

        return Result<Unit, ConvertReason>.Success( Unit.Default );
    }

    private async Task<Result<Unit, ConvertReason>> ConvertImplAsync(
        UniversalDefinition definition,
        CancellationToken cancellationToken = default )
    {
        var outputDirectory = MakeCubaseOutputDirectory( outputBaseDirectory, definition );
        var outputPath = MakeCubaseOutputPath( outputDirectory, definition );

        try
        {
            Directory.CreateDirectory( outputDirectory );

            await using var writer = new LocalTextContentWriter( outputPath );
            var cubaseFacade = new CubaseDefinitionFacade();
            var exportResult = await cubaseFacade.ExportAsync( writer, definition, cancellationToken );

            if( exportResult.IsFailure )
            {
                return Result<Unit, ConvertReason>.Failure(
                    exportResult.Reason switch
                    {
                        ExportReason.SerializationError => ConvertReason.SerializationError,
                        ExportReason.IoError            => ConvertReason.IoError,
                        _                               => ConvertReason.OtherError
                    }
                );
            }
        }
        catch( IOException e )
        {
            return Result<Unit, ConvertReason>.Failure( ConvertReason.IoError, e );
        }
        catch( Exception e )
        {
            return Result<Unit, ConvertReason>.Failure( ConvertReason.OtherError, e );
        }

        return Result<Unit, ConvertReason>.Success( Unit.Default );
    }

    private string MakeCubaseOutputDirectory( string baseDirectory, UniversalDefinition definition )
    {
        return Path.Combine(
            baseDirectory,
            "Cuabase",
            definition.ManufacturerName.Value,
            definition.ProductName.Value
        );
    }

    private static string MakeCubaseOutputPath( string outputDirectory, UniversalDefinition definitions )
    {
        return Path.Combine(
            outputDirectory,
            definitions.ProductName.Value + ".expressionmap"
        );
    }
}
