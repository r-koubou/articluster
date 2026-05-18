using System;
using System.CommandLine;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Services;

using Microsoft.Extensions.Logging;

#pragma warning disable CA2254
#pragma warning disable CA1873

namespace ArtiCluster.Applications.Cli;

internal sealed class CreateDefinitionCommandExecutor : ICommandExecutor
{
    private readonly IUniversalDefinitionFileService service;
    private readonly ILogger<CreateDefinitionCommandExecutor> logger;

    // ReSharper disable once ConvertToPrimaryConstructor
    public CreateDefinitionCommandExecutor(
        IUniversalDefinitionFileService service,
        ILogger<CreateDefinitionCommandExecutor> logger )
    {
        this.service = service;
        this.logger  = logger;
    }

    public Command CreateCommand()
    {
        var outputCreatedPathArgument = new Argument<string>( "path/to/name" );
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

                if( !outputPath.EndsWith( ".yaml" ) )
                {
                    outputPath  += ".yaml";
                }

                return await ExecuteAsync( outputPath );
            }
        );

        return command;
    }

    private async Task<int> ExecuteAsync( string outputPath, CancellationToken cancellationToken = default )
    {
        var result = await service.ExportTemplateAsync( outputPath, cancellationToken );

        if( result.IsFailure )
        {
            var error = result.UnwrapError();

            logger.LogError( $"Failed to create Universal Definition file: {result.Reason}" );

            if( error.Error != null )
            {
                await Console.Error.WriteLineAsync( $"{error.Error.Message}" );
            }

            return 1;
        }

        logger.LogInformation( $"Created Universal Definition file at: {outputPath}" );

        return 0;
    }
}
