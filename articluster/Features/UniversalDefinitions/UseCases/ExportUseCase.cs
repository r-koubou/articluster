using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinitions.Gateways;
using ArtiCluster.Shared.Domain.Articulation.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.UniversalDefinitions.UseCases;

public sealed class ExportInputPort
{
    public IDefinitionExporter Exporter { get; init; }
    public ITextContentWriter ContentWriter { get; init; }
    public UniversalDefinition Source { get; init; }

    // ReSharper disable once ConvertToPrimaryConstructor
    public ExportInputPort( UniversalDefinition source, IDefinitionExporter exporter, ITextContentWriter contentWriter )
    {
        Exporter      = exporter;
        ContentWriter = contentWriter;
        Source        = source;
    }
}

public sealed class ExportUseCase
{
    // ReSharper disable once MemberCanBeMadeStatic.Global
    public async Task<Result<Unit, ExportReason>> ExecuteAsync( ExportInputPort input, CancellationToken cancellationToken = default )
    {
        return await input.Exporter.ExportAsync( input.ContentWriter, input.Source, cancellationToken );
    }
}
