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

public sealed class CakewalkOutputFileService : IExportFileService
{
    private readonly ILoggerFactory loggerFactory;

    public string TargetDawName
        => "Cakewalk";

    // ReSharper disable once ConvertToPrimaryConstructor
    public CakewalkOutputFileService( ILoggerFactory loggerFactory )
    {
        this.loggerFactory = loggerFactory;
    }

    public async Task<Result<Unit, ExportFailureReason>> ExportAsync(
        string outputBaseDirectory,
        IReadOnlyCollection<UniversalDefinition> definitions,
        CancellationToken cancellationToken = default )
    {
        var runner = new FileExportRunner( loggerFactory );
        var namingStrategy = new CakewalkExportNamingStrategy();
        var strategy = new CakewalkFileExportStrategy();
        var collection = new ProductCollection( definitions );

        return await runner.RunAsync(
            outputBaseDirectory,
            collection.Items,
            namingStrategy,
            strategy,
            cancellationToken
        );
    }
}
