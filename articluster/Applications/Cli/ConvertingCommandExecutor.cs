using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services;
using ArtiCluster.Applications.Services.Abstractions;

namespace ArtiCluster.Applications.Cli;

internal sealed class ConvertingCommandExecutor : ICommandExecutor
{
    private readonly IEnumerable<ILocalFileConversionService> services;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ConvertingCommandExecutor( IEnumerable<ILocalFileConversionService> services )
    {
        this.services = services;
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
                    await Console.Error.WriteLineAsync( $"Output directory already exists. ({outputDirectory})" );

                    return 1;
                }

                if( inputDirectory == outputDirectory )
                {
                    await Console.Error.WriteLineAsync( $"Input and Output paths cannot be the same. ({outputDirectory})" );

                    return 1;
                }

                return await ExecuteAsync( inputDirectory,  outputDirectory, services );
            }
        );

        return command;
    }

    private static async Task<int> ExecuteAsync( string inputDirectory, string outputBaseDirectory, IEnumerable<ILocalFileConversionService> convertingServices, CancellationToken cancellationToken = default )
    {
        var importService = new BulkImportUniversalDefinitionService();
        var importResult = await importService.ImportAsync( inputDirectory, cancellationToken );

        if( importResult.IsFailure )
        {
            await Console.Error.WriteLineAsync( $"Failed to import Universal Definitions: {importResult.Reason}" );

            return 1;
        }

        foreach( var service in convertingServices )
        {
            await Console.Out.WriteLineAsync( $"Converting to \"{service.TargetDawName}\" format..." );

            var convertResult = await service.ConvertAsync( outputBaseDirectory, importResult.Unwrap(), cancellationToken );

            if( !convertResult.IsFailure )
            {
                continue;
            }

            await Console.Error.WriteLineAsync( $"Failed to convert (target:{service.TargetDawName}, reason:{convertResult.Reason})" );

            return 1;

        }

        await Console.Out.WriteLineAsync( "Successfully converted." );

        return 0;
    }
}
