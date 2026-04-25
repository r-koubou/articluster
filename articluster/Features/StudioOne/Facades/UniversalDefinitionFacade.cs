using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.StudioOne.Gateways;
using ArtiCluster.Features.StudioOne.Infrastructures;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.StudioOne.Facades;

public sealed class UniversalDefinitionFacade : IStudioOneDefinitionFacade
{
    public async Task<Result<Unit, ExportReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinitionProductSet source,
        CancellationToken cancellationToken = default )
    {
        var exporter = new StudioOneExporter();
        var input = new UseCases.ExportInputPort( source, exporter, writer );
        var useCase = new UseCases.ExportUseCase();

        return await useCase.ExecuteAsync( input, cancellationToken );
    }
}
