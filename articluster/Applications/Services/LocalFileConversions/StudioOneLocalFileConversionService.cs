using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.LocalFileConversions.Runners;
using ArtiCluster.Applications.Services.LocalFileConversions.Strategies;
using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

using Microsoft.Extensions.Logging;

namespace ArtiCluster.Applications.Services.LocalFileConversions;

public sealed class StudioOneLocalFileConversionService : ILocalFileConversionService
{
    private readonly ILoggerFactory loggerFactory;

    public string TargetDawName
        => "Studio One";

    // ReSharper disable once ConvertToPrimaryConstructor
    public StudioOneLocalFileConversionService( ILoggerFactory loggerFactory )
    {
        this.loggerFactory = loggerFactory;
    }

    public async Task<Result<Unit, ConvertFailureReason>> ConvertAsync(
        string outputBaseDirectory,
        IReadOnlyCollection<UniversalDefinition> definitions,
        CancellationToken cancellationToken = default )
    {
        var runner = new LocalFileConversionRunner( loggerFactory );
        var strategy = new StudioOneLocalFileExportStrategy();

        return await runner.RunAsync(
            outputBaseDirectory,
            definitions,
            strategy,
            cancellationToken
        );
    }
}
