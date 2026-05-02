using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.StudioOne.KeySwitches.Contracts;
using ArtiCluster.Features.StudioOne.KeySwitches.Exports;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.StudioOne.KeySwitches.Facades;

public sealed class StudioOneDefinitionFacade
{
    public async Task<Result<Unit, ExportFailureReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinitionProductSet source,
        CancellationToken cancellationToken = default )
    {
        var exporter = new StudioOneExporter();
        return await exporter.ExportAsync( writer, source, cancellationToken );
    }
}
