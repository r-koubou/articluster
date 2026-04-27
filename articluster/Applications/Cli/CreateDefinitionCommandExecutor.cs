using System;
using System.CommandLine;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Features.UniversalDefinitions.Facades;
using ArtiCluster.Shared.Domain.MidiMessages.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
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

        Directory.CreateDirectory( outputDirectory );

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
                        // Note On, Middle C, Velocity 100
                        MidiMessage.Create( 0x90, 60, 100 )
                    ]
                )
            ]
        );

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
