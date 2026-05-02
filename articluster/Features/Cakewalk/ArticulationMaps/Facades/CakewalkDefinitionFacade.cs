using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.Cakewalk.ArticulationMaps.Contracts;
using ArtiCluster.Features.Cakewalk.ArticulationMaps.Exports;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.Cakewalk.ArticulationMaps.Facades;

public sealed class CakewalkDefinitionFacade : ICakewalkDefinitionFacade
{
    public async Task<Result<Unit, ExportFailureReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinitionProductSet source,
        CancellationToken cancellationToken = default )
    {
        var exporter = new CakewalkExporter();
        return await exporter.ExportAsync( writer, source, cancellationToken );
    }
}
