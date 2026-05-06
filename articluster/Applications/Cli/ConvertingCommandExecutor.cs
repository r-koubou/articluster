using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;

using Microsoft.Extensions.Logging;

#pragma warning disable CA2254
#pragma warning disable CA1873

namespace ArtiCluster.Applications.Cli;

internal sealed class ConvertingCommandExecutor : ICommandExecutor
{
    private readonly IEnumerable<ILocalFileConversionService> services;
    private readonly IUniversalDefinitionLocalFileService importService;
    private readonly ILogger<ConvertingCommandExecutor> logger;


    // ReSharper disable once ConvertToPrimaryConstructor
    public ConvertingCommandExecutor(
        IEnumerable<ILocalFileConversionService> services,
        IUniversalDefinitionLocalFileService importService,
        ILogger<ConvertingCommandExecutor> logger )
    {
        this.services      = services;
        this.importService = importService;
        this.logger        = logger;
    }

    public Command CreateCommand()
    {
        var inputDirectoryArgument = new Argument<string>( "input-dir" );
        var outputDirectoryArgument = new Argument<string>( "output-dir" );
        var command = new Command( "convert", "Convert to DAW-specific format." )
        {
            inputDirectoryArgument,
            outputDirectoryArgument
        };

        command.SetAction( async parseResult =>
            {
                var inputDirectory = parseResult.GetValue( inputDirectoryArgument );
                var outputDirectory = parseResult.GetValue( outputDirectoryArgument );

                if( inputDirectory == null || outputDirectory == null )
                {
                    throw new InvalidOperationException( "Input or Output directory is not provided." );
                }

                if( Directory.Exists( outputDirectory ) )
                {
                    logger.LogError( $"Output directory already exists. ({outputDirectory})" );
                    return 1;
                }

                if( inputDirectory == outputDirectory )
                {
                    logger.LogError( $"Input and Output paths cannot be the same. ({outputDirectory})" );
                    return 1;
                }

                return await ExecuteAsync( inputDirectory, outputDirectory );
            }
        );

        return command;
    }

    private async Task<int> ExecuteAsync(
        string inputDirectory,
        string outputBaseDirectory,
        CancellationToken cancellationToken = default )
    {
        var importResult = await importService.ImportAsync( inputDirectory, cancellationToken );

        if( importResult.IsFailure )
        {
            logger.LogError( $"Failed to import Universal Definitions: {importResult.Reason}" );

            return 1;
        }

        foreach( var service in services )
        {
            logger.LogInformation( $"Convert to \"{service.TargetDawName}\" format..." );

            var convertResult = await service.ConvertAsync( outputBaseDirectory, importResult.Unwrap(), cancellationToken );

            if( !convertResult.IsFailure )
            {
                continue;
            }

            logger.LogError( $"Failed to convert (target:{service.TargetDawName}, reason:{convertResult.Reason})" );

            return 1;
        }

        logger.LogInformation( "Successfully converted." );

        return 0;
    }
}
