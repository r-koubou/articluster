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
    public Command CreateCommand()
    {
        var inputDirectoryArgument = new Argument<string>( "input-dir" );
        var outputDirectoryArgument = new Argument<string>( "output-dir" );
        var command = new Command( "convert", "Convert to DAW-specific format." )
        {
            inputDirectoryArgument,
            outputDirectoryArgument
        };
        command.SetAction( parseResult =>
            {
                var inputDirectory = parseResult.GetValue( inputDirectoryArgument );
                var outputDirectory = parseResult.GetValue( outputDirectoryArgument );

                if( inputDirectory == null || outputDirectory == null )
                {
                    throw new InvalidOperationException( "Input or Output directory is not provided." );
                }

                if( Directory.Exists( outputDirectory ) )
                {
                    Console.Error.WriteLineAsync( "Output directory already exists." );
                    Environment.Exit( 1 );

                    return;
                }

                if( inputDirectory == outputDirectory )
                {
                    Console.Error.WriteLine( "Input and Output paths cannot be the same." );
                    Environment.Exit( 1 );

                    return;
                }

                var convertingServices = new List<IConvertingService>
                {
                    new StudioOneConvertingService( outputDirectory )
                };

                Task.Run( async () => await ExecuteAsync( inputDirectory, convertingServices ) ).Wait();
            }
        );

        return command;
    }

    private static async Task ExecuteAsync( string inputDirectory, IReadOnlyCollection<IConvertingService> convertingServices, CancellationToken cancellationToken = default )
    {
        var importService = new BulkImportUniversalDefinitionService();
        var importResult = await importService.ImportAsync( inputDirectory, cancellationToken );

        if( importResult.IsFailure )
        {
            await Console.Error.WriteLineAsync( $"Failed to import Universal Definitions: {importResult.Reason}" );
            Environment.Exit( 1 );

            return;
        }

        foreach( var service in convertingServices )
        {
            await Console.Out.WriteLineAsync( $"Converting to \"{service.TargetDawName}\" format..." );

            var convertResult = await service.ConvertAsync( importResult.Unwrap(), cancellationToken );

            if( !convertResult.IsFailure )
            {
                continue;
            }

            await Console.Error.WriteLineAsync( $"Failed to convert Studio One: {convertResult.Reason}" );
            Environment.Exit( 1 );

            return;

        }

        await Console.Out.WriteLineAsync( "Successfully converted." );
    }
}
