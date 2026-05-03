using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.LocalFileConversions.Runners;
using ArtiCluster.Applications.Services.LocalFileConversions.Strategies;
using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions;

namespace ArtiCluster.Applications.Services.LocalFileConversions;

public sealed class CubaseLocalFileConversionService : ILocalFileConversionService
{
    public string TargetDawName
        => "Cubase";

    public async Task<Result<Unit, ConvertReason>> ConvertAsync(
        string outputBaseDirectory,
        UniversalDefinitionProductCollection definitions,
        CancellationToken cancellationToken = default )
    {
        var runner = new LocalFileConversionRunner();
        var strategy = new CubaseLocalFileExportStrategy();

        return await runner.RunAsync(
            outputBaseDirectory,
            definitions.EnumerateDefinitions(),
            strategy,
            cancellationToken
        );
    }
}
