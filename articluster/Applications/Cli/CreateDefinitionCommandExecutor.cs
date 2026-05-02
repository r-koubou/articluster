using System;
using System.CommandLine;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services;
using ArtiCluster.Features.UniversalDefinitions.Facades;
using ArtiCluster.Shared.IO.Local;

namespace ArtiCluster.Applications.Cli;

internal sealed class CreateDefinitionCommandExecutor : ICommandExecutor
{
    public Command CreateCommand()
    {
        var outputCreatedPathArgument = new Argument<string>( "path/to/output.yaml" );
        var command = new Command( "new", "Create a Universal Definition file" )
        {
            outputCreatedPathArgument
        };
        command.SetAction( async parseResult =>
            {
                var outputPath = parseResult.GetValue( outputCreatedPathArgument );

                if( outputPath == null )
                {
                    throw new InvalidOperationException( "Output path is required." );
                }

                return await ExecuteAsync( outputPath );
            }
        );

        return command;
    }

    private static async Task<int> ExecuteAsync( string outputPath, CancellationToken cancellationToken = default )
    {
        var outputDirectory = Path.GetDirectoryName( outputPath );

        if( outputDirectory == null )
        {
            await Console.Error.WriteLineAsync( "Cannot determine output directory from the provided path." );

            return 1;
        }

        // outputPath has parent directory, create it if it doesn't exist
        if( outputDirectory.Length > 0 )
        {
            Directory.CreateDirectory( outputDirectory );
        }

        var definition = UniversalDefinitionService.CreateTemplate();

        var facade = new UniversalDefinitionFacade();
        await using var writer = new LocalTextContentWriter( outputPath );

        var result = await facade.ExportAsync( writer, definition, cancellationToken );

        if( result.IsFailure )
        {
            await Console.Error.WriteLineAsync( $"Failed to create Universal Definition file: {result.Reason}" );

            return 1;
        }

        Console.WriteLine( $"Created Universal Definition file at: {outputPath}" );

        return 0;
    }
}
