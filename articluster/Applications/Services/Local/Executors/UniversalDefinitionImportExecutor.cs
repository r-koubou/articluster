using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.Abstractions.Executors;
using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinitions.Facades;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Local;

using Microsoft.Extensions.Logging;

using FacadeImportFailureReason = ArtiCluster.Features.UniversalDefinitions.Contracts.ImportFailureReason;

namespace ArtiCluster.Applications.Services.Local.Executors;

public partial class UniversalDefinitionImportExecutor
    : IImportExecutor<IReadOnlyCollection<UniversalDefinition>>
{
    private readonly ILogger<UniversalDefinitionImportExecutor> logger;

    // ReSharper disable once ConvertToPrimaryConstructor
    public UniversalDefinitionImportExecutor( ILoggerFactory loggerFactory )
    {
        logger = loggerFactory.CreateLogger<UniversalDefinitionImportExecutor>();
    }

    public async Task<Result<IReadOnlyCollection<UniversalDefinition>, ImportFailureReason>> ExecuteAsync(
        string baseInputDirectory,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var facade = new UniversalDefinitionFacade();
            var definitionFiles = Directory.GetFiles( baseInputDirectory, "*.yaml", SearchOption.AllDirectories );
            var definitions = new List<UniversalDefinition>();

            foreach( var file in definitionFiles )
            {
                using var reader = new LocalTextContentReader( file );
                var importResult = await facade.ImportAsync( reader, cancellationToken );

                if( importResult.IsFailure )
                {
                    var reason = importResult.Reason switch
                    {
                        FacadeImportFailureReason.UnsupportedFormatVersion => ImportFailureReason.UnsupportedFormatVersion,
                        FacadeImportFailureReason.DeserializationError     => ImportFailureReason.DeserializationError,
                        FacadeImportFailureReason.IoError                  => ImportFailureReason.IoError,
                        _                                                  => ImportFailureReason.OtherError
                    };

                    LogFailedToImportDefinitionFromFileFile( file, reason, importResult.UnwrapError().Error );

                    return Result<IReadOnlyCollection<UniversalDefinition>, ImportFailureReason>.Failure( reason );
                }

                definitions.Add( importResult.Unwrap() );
            }

            LogImportedSuccessfullyCount( definitions.Count );

            return Result<IReadOnlyCollection<UniversalDefinition>, ImportFailureReason>.Success( definitions );
        }
        catch( IOException e )
        {
            return Result<IReadOnlyCollection<UniversalDefinition>, ImportFailureReason>.Failure(
                ImportFailureReason.IoError,
                e
            );
        }
        catch( Exception e )
        {
            return Result<IReadOnlyCollection<UniversalDefinition>, ImportFailureReason>.Failure(
                ImportFailureReason.OtherError,
                e
            );
        }
    }

    #region Logging
    [LoggerMessage( LogLevel.Debug, "Importing file: {File}" )]
    partial void LogImportingFileFile( string file );

    [LoggerMessage( LogLevel.Information, "Imported successfully {Count} definitions" )]
    partial void LogImportedSuccessfullyCount( int count );

    [LoggerMessage( LogLevel.Critical, "Failed to import definition from file {File}. Reason: {Reason}" )]
    partial void LogFailedToImportDefinitionFromFileFile( string file, ImportFailureReason reason, Exception? exception );
    #endregion
}
