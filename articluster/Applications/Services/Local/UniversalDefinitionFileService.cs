using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.Abstractions.Collectors;
using ArtiCluster.Applications.Services.Abstractions.Services;
using ArtiCluster.Applications.Services.Local.Executors;
using ArtiCluster.Applications.Services.Local.Runners;
using ArtiCluster.Applications.Services.Local.Strategies;
using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

using Microsoft.Extensions.Logging;

namespace ArtiCluster.Applications.Services.Local;

public sealed class UniversalDefinitionFileService : IUniversalDefinitionFileService
{
    private readonly ILoggerFactory loggerFactory;

    // ReSharper disable once ConvertToPrimaryConstructor
    public UniversalDefinitionFileService( ILoggerFactory loggerFactory )
    {
        this.loggerFactory = loggerFactory;
    }

    public async Task<Result<IReadOnlyCollection<UniversalDefinition>, ImportFailureReason>> ImportAsync( string definitionsDirectory, CancellationToken cancellationToken = default )
    {
        var executor = new UniversalDefinitionImportExecutor( loggerFactory );
        return await executor.ExecuteAsync( definitionsDirectory, cancellationToken );
    }

    public async Task<Result<Unit, ExportFailureReason>> ExportAsync( string outputPath, UniversalDefinition definition, CancellationToken cancellationToken = default )
    {
        var fullpath = Path.GetFullPath( outputPath );
        var outputDirectory = Path.GetDirectoryName( fullpath );

        ArgumentNullException.ThrowIfNull( outputDirectory, nameof( outputPath ) );

        var runner = new FileExportRunner( loggerFactory );
        var namingStrategy = new UniversalDefinitionTemplateExportNamingStrategy();
        var strategy = new UniversalDefinitionFileExportStrategy();
        var factory = new UniversalDefinitionExportedFileEntryFactory();
        var collector = new InMemoryExportedFileCollector();

        var result = await runner.RunAsync(
            outputDirectory,
            [ definition ],
            namingStrategy,
            strategy,
            factory,
            collector,
            cancellationToken
        );

        return result;
    }

    public async Task<Result<Unit, ExportFailureReason>> ExportTemplateAsync( string outputPath, CancellationToken cancellationToken = default )
    {
        var patchName = Path.GetFileNameWithoutExtension( outputPath );
        var definition = UniversalDefinition.CreateTemplate( patchName: patchName );

        return await ExportAsync( outputPath, definition, cancellationToken );
    }
}
