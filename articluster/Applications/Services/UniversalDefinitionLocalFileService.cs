using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinitions.Facades;
using ArtiCluster.Shared.Domain.MidiMessages.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Local;

using Microsoft.Extensions.Logging;

using FacadeImportFailureReason = ArtiCluster.Features.UniversalDefinitions.Contracts.ImportFailureReason;
using FacadeExportFailureReason = ArtiCluster.Features.UniversalDefinitions.Contracts.ExportFailureReason;

namespace ArtiCluster.Applications.Services;

public sealed class UniversalDefinitionLocalFileService : IUniversalDefinitionLocalFileService
{
    private readonly ILogger<UniversalDefinitionLocalFileService> logger;

    // ReSharper disable once ConvertToPrimaryConstructor
    public UniversalDefinitionLocalFileService( ILogger<UniversalDefinitionLocalFileService> logger )
    {
        this.logger = logger;
    }

    public async Task<Result<UniversalDefinitionProductCollection, ImportFailureReason>> ImportAsync( string definitionsDirectory, CancellationToken cancellationToken = default )
    {
        try
        {
            var facade = new UniversalDefinitionFacade();
            var definitionFiles = Directory.GetFiles( definitionsDirectory, "*.yaml", SearchOption.AllDirectories );
            var definitions = new List<UniversalDefinition>();

            foreach( var file in definitionFiles )
            {
                using var reader = new LocalTextContentReader( file );
                var importResult = await facade.ImportAsync( reader, cancellationToken );

                if( importResult.IsFailure )
                {
                    var reason = importResult.Reason switch
                    {
                        FacadeImportFailureReason.DeserializationError => ImportFailureReason.DeserializationError,
                        FacadeImportFailureReason.IoError              => ImportFailureReason.IoError,
                        _                                              => ImportFailureReason.OtherError
                    };

                    return Result<UniversalDefinitionProductCollection, ImportFailureReason>.Failure( reason );
                }

                definitions.Add( importResult.Unwrap() );
            }

            return Result<UniversalDefinitionProductCollection, ImportFailureReason>.Success(
                new UniversalDefinitionProductCollection( definitions )
            );
        }
        catch( IOException e )
        {
            return Result<UniversalDefinitionProductCollection, ImportFailureReason>.Failure(
                ImportFailureReason.IoError,
                e
            );
        }
        catch( Exception e )
        {
            return Result<UniversalDefinitionProductCollection, ImportFailureReason>.Failure(
                ImportFailureReason.OtherError,
                e
            );
        }
    }

    public async Task<Result<Unit, ExportFailureReason>> ExportAsync( string outputPath, UniversalDefinition definition, CancellationToken cancellationToken = default )
    {
        logger.LogInformation( "Export begin" );

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
        catch( Exception )
        {
            return Result<Unit, ExportFailureReason>.Failure( ExportFailureReason.OtherError );
        }
    }

    public async Task<Result<Unit, ExportFailureReason>> ExportTemplateAsync( string outputPath, CancellationToken cancellationToken = default )
    {
        var definition = UniversalDefinition.Create(
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

        return await ExportAsync( outputPath, definition, cancellationToken );
    }
}
