using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.Cubase.ExpressionMaps.Contracts;
using ArtiCluster.Features.Cubase.ExpressionMaps.Exports;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.Cubase.ExpressionMaps.Facades;

/// <summary>
/// Exporter for Cubase 15 and later
/// </summary>
public sealed class Cubase15DefinitionFacade : ICubaseDefinitionFacade<UniversalDefinition>
{
    public async Task<Result<Unit, ExportFailureReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinition source,
        CancellationToken cancellationToken = default )
    {
        var exporter = new Cubase15Exporter();
        return await exporter.ExportAsync( writer, source, cancellationToken );
    }
}
