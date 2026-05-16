using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.Cubase.ExpressionMaps.Contracts;
using ArtiCluster.Features.Cubase.ExpressionMaps.Exports;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.Cubase.ExpressionMaps.Facades;

public sealed class CubaseDefinitionFacade : ICubaseDefinitionFacade
{
    public async Task<Result<Unit, ExportFailureReason>> ExportAsync(
        ITextContentWriter writer,
        SeparatedArticulationGroupSet source,
        CancellationToken cancellationToken = default )
    {
        var exporter = new CubaseExporter();
        return await exporter.ExportAsync( writer, source, cancellationToken );
    }
}
