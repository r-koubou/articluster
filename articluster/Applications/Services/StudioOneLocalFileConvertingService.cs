using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Commons;
using ArtiCluster.Features.StudioOne.KeySwitchDefinitions.Facades;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Local;

namespace ArtiCluster.Applications.Services;

public sealed class StudioOneLocalFileConvertingService : ILocalFileConvertingService
{
    public string TargetDawName
        => "Studio One";

    public async Task<Result<Unit, ConvertReason>> ConvertAsync( string outputBaseDirectory, UniversalDefinitionProductCollection definitions, CancellationToken cancellationToken = default )
    {
        // Export
        foreach( var x in definitions.Items )
        {
            var outputDirectory = MakeStudioOneOutputDirectory( outputBaseDirectory, x );
            var outputPath = MakeStudioOneOutputPath( outputDirectory, x );

            try
            {
                Directory.CreateDirectory( outputDirectory );

                await using var writer = new LocalTextContentWriter( outputPath );
                var facade = new StudioOneDefinitionFacade();
                var exportResult = await facade.ExportAsync( writer, x, cancellationToken );

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
        }

        return Result<Unit, ConvertReason>.Success( Unit.Default );
    }

    private static string MakeStudioOneOutputDirectory( string baseDirectory, UniversalDefinitionProductSet definitions )
    {
        return Path.Combine(
            baseDirectory,
            "StudioOne",
            definitions.ManufacturerName.Value,
            definitions.ProductName.Value
        );
    }

    private static string MakeStudioOneOutputPath( string outputDirectory, UniversalDefinitionProductSet definitions )
    {
        return Path.Combine(
            outputDirectory,
            definitions.ProductName.Value + ".keyswitch"
        );
    }
}
