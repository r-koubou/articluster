using System;
using System.CommandLine;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;

namespace ArtiCluster.Applications.Cli;

internal sealed class CreateDefinitionCommandExecutor : ICommandExecutor
{
    private readonly IUniversalDefinitionLocalFileService service;

    // ReSharper disable once ConvertToPrimaryConstructor
    public CreateDefinitionCommandExecutor( IUniversalDefinitionLocalFileService service )
    {
        this.service = service;
    }

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

                return await ExecuteAsync( outputPath, service );
            }
        );

        return command;
    }

    private static async Task<int> ExecuteAsync( string outputPath, IUniversalDefinitionLocalFileService service, CancellationToken cancellationToken = default )
    {
        var result = await service.ExportTemplateAsync( outputPath, cancellationToken );

        if( result.IsFailure )
        {
            var error = result.UnwrapError();

            await Console.Error.WriteLineAsync( $"Failed to create Universal Definition file: {result.Reason}" );

            if( error.Error != null )
            {
                await Console.Error.WriteLineAsync( $"{error.Error.Message}" );
            }

            return 1;
        }

        Console.WriteLine( $"Created Universal Definition file at: {outputPath}" );

        return 0;
    }
}
