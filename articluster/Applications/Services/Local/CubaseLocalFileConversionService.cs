using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Local;
using ArtiCluster.Applications.Services.Local.Runners;
using ArtiCluster.Applications.Services.Local.Strategies;
using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

using Microsoft.Extensions.Logging;

namespace ArtiCluster.Applications.Services.Local;

public sealed class CubaseLocalFileConversionService : ILocalFileConversionService
{
    private readonly ILoggerFactory loggerFactory;

    public string TargetDawName
        => "Cubase";

    // ReSharper disable once ConvertToPrimaryConstructor
    public CubaseLocalFileConversionService( ILoggerFactory loggerFactory )
    {
        this.loggerFactory = loggerFactory;
    }

    public async Task<Result<Unit, ConvertFailureReason>> ConvertAsync(
        string outputBaseDirectory,
        IReadOnlyCollection<UniversalDefinition> definitions,
        CancellationToken cancellationToken = default )
    {
        var runner = new LocalFileConversionRunner( loggerFactory );
        var namingStrategy = new CubaseLocalOutputNamingStrategy();
        var strategy = new CubaseLocalFileExportStrategy();
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
