using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Exports;
using ArtiCluster.Applications.Services.Abstractions.Exports.Collectors;
using ArtiCluster.Applications.Services.Abstractions.Imports;
using ArtiCluster.Applications.Services.Abstractions.Imports.Collectors;
using ArtiCluster.Applications.Services.Abstractions.Services;
using ArtiCluster.Applications.Services.Local.Exports.Runners;
using ArtiCluster.Applications.Services.Local.Exports.Strategies;
using ArtiCluster.Applications.Services.Local.Imports.Runners;
using ArtiCluster.Applications.Services.Local.Imports.Strategies;
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
        var runner = new FileImportRunner( loggerFactory );
        var collector = new InMemoryImportedFileCollector();
        var result = await runner.RunAsync(
            definitionsDirectory,
            new UniversalDefinitionFileImportStrategy(),
            new UniversalDefinitionImportNamingStrategy(),
            new UniversalDefinitionImportedFileEntryFactory(),
            collector,
            cancellationToken
        );

        if( result.IsFailure )
        {
            var error = result.UnwrapError();
            return Result<IReadOnlyCollection<UniversalDefinition>, ImportFailureReason>.Failure( error.Reason, error.Error );
        }

        var definitions =
            collector.Items
                     .Select( x => x.Definition )
                     .ToList();

        return Result<IReadOnlyCollection<UniversalDefinition>, ImportFailureReason>.Success( definitions );
    }

    public async Task<Result<Unit, ExportFailureReason>> ExportAsync( string outputDirectory, UniversalDefinition definition, CancellationToken cancellationToken = default )
    {
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

    public async Task<Result<Unit, ExportFailureReason>> ExportTemplateAsync( string outputDirectory, string patchName, CancellationToken cancellationToken = default )
    {
        var definition = UniversalDefinition.CreateTemplate( patchName: patchName );

        return await ExportAsync( outputDirectory, definition, cancellationToken );
    }
}
