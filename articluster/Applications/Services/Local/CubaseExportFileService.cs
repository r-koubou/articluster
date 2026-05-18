using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.Abstractions.Services;
using ArtiCluster.Applications.Services.Local.Runners;
using ArtiCluster.Applications.Services.Local.Strategies;
using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

using Microsoft.Extensions.Logging;

namespace ArtiCluster.Applications.Services.Local;

public sealed class CubaseExportFileService : IExportFileService
{
    private readonly ILoggerFactory loggerFactory;

    public string TargetDawName
        => "Cubase";

    // ReSharper disable once ConvertToPrimaryConstructor
    public CubaseExportFileService( ILoggerFactory loggerFactory )
    {
        this.loggerFactory = loggerFactory;
    }

    public async Task<Result<Unit, ExportFailureReason>> ExportAsync(
        string outputBaseDirectory,
        IReadOnlyCollection<UniversalDefinition> definitions,
        CancellationToken cancellationToken = default )
    {
        var runner = new FileExportRunner( loggerFactory );
        var namingStrategy = new CubaseExportNamingStrategy();
        var strategy = new CubaseFileExportStrategy();
        var collection = new SeparatedArticulationGroupCollection( definitions );

        return await runner.RunAsync(
            outputBaseDirectory,
            collection.Items,
            namingStrategy,
            strategy,
            cancellationToken
        );
    }
}
