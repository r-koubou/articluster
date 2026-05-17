using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.Cubase15.ExpressionMaps.Contracts;
using ArtiCluster.Features.Cubase15.ExpressionMaps.Exports;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.Cubase15.ExpressionMaps.Facades;

/// <summary>
/// Exporter for Cubase 15 and later
/// </summary>
public sealed class CubaseDefinitionFacade : ICubaseDefinitionFacade<UniversalDefinition>
{
    public async Task<Result<Unit, ExportFailureReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinition source,
        CancellationToken cancellationToken = default )
    {
        var exporter = new CubaseExporter();
        return await exporter.ExportAsync( writer, source, cancellationToken );
    }
}
