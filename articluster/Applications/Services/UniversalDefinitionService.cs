using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.LocalFileConversions;
using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinitions.Facades;
using ArtiCluster.Shared.Domain.MidiMessages.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Local;

using FacadeExportFailureReason = ArtiCluster.Features.UniversalDefinitions.Contracts.ExportFailureReason;

namespace ArtiCluster.Applications.Services;

public static class UniversalDefinitionService
{
    public static UniversalDefinition CreateTemplate()
        => UniversalDefinition.Create(
            id: Guid.NewGuid(),
            author: "Example Author",
            manufacturerName: "Example Manufacturer",
            productName: "Example Product",
            patchName: "Example Patch",
            description: "Example Description",
            articulations:
            [
                Articulation.Create(
                    name: "Articulation Name",
                    midiMessages:
                    [
                        MidiMessage.Create( 0x90, 60, 100 )
                    ]
                )
            ]
        );

    public static async Task<Result<Unit, ExportFailureReason>> WriteTemplateAsync( string outputPath, CancellationToken cancellationToken = default )
    {
        try
        {
            var outputDirectory = Path.GetDirectoryName( outputPath );

            if( outputDirectory == null )
            {
                throw new ArgumentException( $"Cannot determine output directory from the provided path : {outputPath} )", paramName: nameof( outputPath ) );
            }

            // outputPath has parent directory, create it if it doesn't exist
            if( outputDirectory.Length > 0 )
            {
                Directory.CreateDirectory( outputDirectory );
            }

            var definition = CreateTemplate();

            var facade = new UniversalDefinitionFacade();
            await using var writer = new LocalTextContentWriter( outputPath );

            var result = await facade.ExportAsync( writer, definition, cancellationToken );

            if( result.IsFailure )
            {
                return result.MapError( reason => reason switch
                    {
                        FacadeExportFailureReason.SerializationError => ExportFailureReason.SerializationError,
                        FacadeExportFailureReason.IoError            => ExportFailureReason.IoError,
                        _                                            => ExportFailureReason.OtherError
                    }
                );
            }

            return Result<Unit, ExportFailureReason>.Success( Unit.Default );
        }
        catch( IOException )
        {
            return Result<Unit, ExportFailureReason>.Failure( ExportFailureReason.IoError );
        }
        catch( Exception e )
        {
            return Result<Unit, ExportFailureReason>.Failure( ExportFailureReason.OtherError );
        }
    }
}
