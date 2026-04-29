using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.Gateways;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.UseCases;

public sealed class ExportInputPort
{
    public UniversalDefinitionProductSet Source { get; init; }
    public IDefinitionExporter Exporter { get; init; }
    public ITextContentWriter ContentWriter { get; init; }

    // ReSharper disable once ConvertToPrimaryConstructor
    public ExportInputPort( UniversalDefinitionProductSet source, IDefinitionExporter exporter, ITextContentWriter contentWriter )
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
