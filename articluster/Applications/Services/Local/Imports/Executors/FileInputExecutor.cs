using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Imports;
using ArtiCluster.Applications.Services.Abstractions.Imports.Collectors;
using ArtiCluster.Applications.Services.Abstractions.Imports.Executors;
using ArtiCluster.Applications.Services.Abstractions.Imports.Strategies;
using ArtiCluster.Commons;
using ArtiCluster.Shared.IO.Local;

using Microsoft.Extensions.Logging;

namespace ArtiCluster.Applications.Services.Local.Imports.Executors;

public partial class FileInputExecutor<TTarget> : IImportExecutor<TTarget>
{
    private readonly ILogger<FileInputExecutor<TTarget>> logger;

    // ReSharper disable once ConvertToPrimaryConstructor
    public FileInputExecutor( ILoggerFactory loggerFactory )
    {
        logger = loggerFactory.CreateLogger<FileInputExecutor<TTarget>>();
    }

    public async Task<Result<Unit, ImportFailureReason>> ExecuteAsync(
        string inputDirectory,
        IImportStrategy<TTarget> strategy,
        IImportNamingStrategy namingStrategy,
        IImportedFileEntryFactory<TTarget>? entryFactory = null,
        IImportedFileCollector? collector = null,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var searchPattern = namingStrategy.GetInputFilePattern();
            var searchOption = namingStrategy.GetSearchOption();
            var files = Directory.GetFiles( inputDirectory, searchPattern, searchOption );
            var succeededImports = 0;

            foreach( var x in files )
            {
                LogImportingFileFile( x );

                using var contentReader = new LocalTextContentReader( x );
                var result = await strategy.ImportAsync( contentReader, cancellationToken );

                if( result.IsFailure )
                {
                    var error = result.UnwrapError();

                    LogFailedToImportDefinitionFromFileFile( x, error.Reason, error.Error );

                    return Result<Unit, ImportFailureReason>.Failure( error.Reason, error.Error );
                }

                // ReSharper disable once InvertIf
                if( collector != null && entryFactory != null )
                {
                    var entry = entryFactory.Create( x, result.Unwrap() );
                    await collector.CollectAsync( entry, cancellationToken );
                }

                succeededImports++;
            }

            LogImportedSuccessfullyCount( succeededImports );

            return Result<Unit, ImportFailureReason>.Success( Unit.Default );
        }
        catch( IOException e )
        {
            LogAnUnexpectedErrorOccurredWhileImportingDefinitionsFromDirectoryInputdirectory( inputDirectory, e );
            return Result<Unit, ImportFailureReason>.Failure( ImportFailureReason.IoError, e );
        }
        catch( Exception e )
        {
            LogAnUnexpectedErrorOccurredWhileImportingDefinitionsFromDirectoryInputdirectory( inputDirectory, e );
            return Result<Unit, ImportFailureReason>.Failure( ImportFailureReason.OtherError, e );
        }
    }

    #region Logging
    [LoggerMessage( LogLevel.Debug, "Importing file: {File}" )]
    partial void LogImportingFileFile( string file );

    [LoggerMessage( LogLevel.Information, "Imported successfully {Count} definitions" )]
    partial void LogImportedSuccessfullyCount( int count );

    [LoggerMessage( LogLevel.Critical, "Failed to import definition from file {File}. Reason: {Reason}" )]
    partial void LogFailedToImportDefinitionFromFileFile( string file, ImportFailureReason reason, Exception? exception );

    [LoggerMessage( LogLevel.Error, "An unexpected error occurred while importing definitions from directory {InputDirectory}" )]
    partial void LogAnUnexpectedErrorOccurredWhileImportingDefinitionsFromDirectoryInputdirectory( string inputDirectory, Exception exception );
    #endregion
}
