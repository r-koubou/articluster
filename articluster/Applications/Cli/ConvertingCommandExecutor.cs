using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.Abstractions.MarkdownExports.Services;
using ArtiCluster.Applications.Services.Abstractions.Models;
using ArtiCluster.Applications.Services.Abstractions.Services;
using ArtiCluster.Commons;

using Microsoft.Extensions.Logging;

#pragma warning disable CA2254
#pragma warning disable CA1873

namespace ArtiCluster.Applications.Cli;

internal sealed class ConvertingCommandExecutor : ICommandExecutor
{
    private readonly IEnumerable<IExportFileService> services;
    private readonly IUniversalDefinitionFileService importService;
    private readonly IMarkdownExportIndexFileService markdownExportIndexFileService;
    private readonly ILogger<ConvertingCommandExecutor> logger;


    // ReSharper disable once ConvertToPrimaryConstructor
    public ConvertingCommandExecutor(
        IEnumerable<IExportFileService> services,
        IUniversalDefinitionFileService importService,
        IMarkdownExportIndexFileService markdownExportIndexFileService,
        ILogger<ConvertingCommandExecutor> logger )
    {
        this.services                             = services;
        this.importService                        = importService;
        this.markdownExportIndexFileService       = markdownExportIndexFileService;
        this.logger                               = logger;
    }

    public Command CreateCommand()
    {
        var inputDirectoryArgument = new Argument<string>( "input-dir" );
        var outputDirectoryArgument = new Argument<string>( "output-dir" );

        var overwriteOption = new Option<bool>( "-o", "--overwrite" )
        {
            Description = "Overwrite output directory if it already exists."
        };

        var outputMarkdownDirectoryOption = new Option<DirectoryInfo>( "-m", "--output-markdown-list-dir" )
        {
            Description = "When specifying this option, please provide the directory path managed by the static site generator. The same applies to specifying the directory path for `output-dir`."
        };

        var command = new Command( "convert", "Convert to DAW-specific format." )
        {
            inputDirectoryArgument,
            outputDirectoryArgument,
            overwriteOption,
            outputMarkdownDirectoryOption,
        };

        command.SetAction( async parseResult =>
            {
                var inputDirectory = parseResult.GetValue( inputDirectoryArgument );
                var outputDirectory = parseResult.GetValue( outputDirectoryArgument );
                var outputMarkdownDirectory = parseResult.GetValue( outputMarkdownDirectoryOption );

                if( inputDirectory == null || outputDirectory == null )
                {
                    throw new InvalidOperationException( "Input or Output directory is not provided." );
                }

                var allowOverwrite = parseResult.GetValue( overwriteOption );

                if( Directory.Exists( outputDirectory ) )
                {
                    if( !allowOverwrite )
                    {
                        logger.LogError( $"Output directory already exists. ({outputDirectory})" );
                        return 1;
                    }

                    logger.LogInformation( $"Existing files in the output directory will be overwritten. ({outputDirectory})" );
                }

                if( inputDirectory == outputDirectory )
                {
                    logger.LogError( $"Input and Output paths cannot be the same. ({outputDirectory})" );
                    return 1;
                }

                return await ExecuteAsync( inputDirectory, outputDirectory, outputMarkdownDirectory );
            }
        );

        return command;
    }

    private async Task<int> ExecuteAsync(
        string inputDirectory,
        string outputBaseDirectory,
        DirectoryInfo? outputMarkdownDir,
        CancellationToken cancellationToken = default )
    {
        var importResult = await importService.ImportAsync( inputDirectory, cancellationToken );

        if( importResult.IsFailure )
        {
            logger.LogCritical( $"Failed to import Universal Definitions: {importResult.Reason}" );

            return 1;
        }

        foreach( var service in services )
        {
            var targetDawName = service.TargetDawName;

            logger.LogInformation( $"{targetDawName}: Converting..." );

            var convertResult = await service.ExportAsync( outputBaseDirectory, importResult.Unwrap(), cancellationToken );

            if( convertResult.IsFailure )
            {
                logger.LogCritical( $"Failed to convert (target:{service.TargetDawName}, reason:{convertResult.Reason})" );

                return 1;
            }

            // ReSharper disable once InvertIf
            if( outputMarkdownDir != null )
            {
                logger.LogInformation( $"{targetDawName}: Generate Markdown for Static Site Generator" );
                logger.LogDebug( $"Output Markdown Directory: {outputMarkdownDir.FullName}" );

                var result = await OutputListMarkdown( outputMarkdownDir, cancellationToken, convertResult );

                if( result != 0 )
                {
                    logger.LogCritical( $"{targetDawName}: Failed to generate Markdown Output Directory. (result={result})" );
                    return result;
                }
            }
        }

        logger.LogInformation( "Successfully converted." );

        return 0;
    }

    private async Task<int> OutputListMarkdown(
        DirectoryInfo outputMarkdownDir,
        CancellationToken cancellationToken,
        Result<IReadOnlyCollection<ExportedFileEntry>, ExportFailureReason> convertResult )
    {
        var markdownExportResult = await markdownExportIndexFileService.ExportAsync(
            outputMarkdownDir.FullName,
            convertResult.Unwrap(),
            cancellationToken
        );

        // ReSharper disable once InvertIf
        if( markdownExportResult.IsFailure )
        {
            logger.LogCritical( $"Failed to export markdown index file (reason:{markdownExportResult.Reason})" );

            return 1;
        }

        return 0;
    }
}
