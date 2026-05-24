using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.Abstractions.Executors;
using ArtiCluster.Applications.Services.Abstractions.Services;
using ArtiCluster.Applications.Services.Local.Executors;
using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinitions.Facades;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Local;

using Microsoft.Extensions.Logging;

using FacadeExportFailureReason = ArtiCluster.Features.UniversalDefinitions.Contracts.ExportFailureReason;

namespace ArtiCluster.Applications.Services.Local;

public sealed class UniversalDefinitionFileService : IUniversalDefinitionFileService
{
    private readonly ILogger<UniversalDefinitionFileService> logger;

    private readonly IUniversalDefinitionImportExecutor importExecutor;

    // ReSharper disable once ConvertToPrimaryConstructor
    public UniversalDefinitionFileService(
        ILogger<UniversalDefinitionFileService> logger,
        IUniversalDefinitionImportExecutor importExecutor )
    {
        this.logger         = logger;
        this.importExecutor = importExecutor;
    }

    public async Task<Result<IReadOnlyCollection<UniversalDefinition>, ImportFailureReason>> ImportAsync( string definitionsDirectory, CancellationToken cancellationToken = default )
    {
        logger.LogInformation( "Import begin" );
        return await importExecutor.ExecuteAsync( definitionsDirectory, cancellationToken );
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
        var definition = UniversalDefinition.CreateTemplate();

        return await ExportAsync( outputPath, definition, cancellationToken );
    }
}
