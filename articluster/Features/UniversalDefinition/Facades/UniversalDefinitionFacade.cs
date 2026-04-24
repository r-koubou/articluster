using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinition.Gateways;
using ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml;
using ArtiCluster.Features.UniversalDefinition.UseCases;
using ArtiCluster.Shared.Domain.Articulation.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.UniversalDefinition.Facades;

public sealed class UniversalDefinitionFacade : IUniversalDefinitionFacade
{
    public async Task<Result<Articulation, ImportReason>> ImportAsync( ITextContentReader reader, CancellationToken cancellationToken = default )
    {
        var importer = new YamlImporter();
        var useCase = new ImportUseCase();
        var input = new ImportInputPort( importer, reader );

        return await useCase.ExecuteAsync( input, cancellationToken );
    }

    public async Task<Result<Unit, ExportReason>> ExportAsync( ITextContentWriter writer, Articulation source, CancellationToken cancellationToken = default )
    {
        var exporter = new YamlExporter();
        var useCase = new ExportUseCase();
        var input = new ExportInputPort( source, exporter, writer );

        return await useCase.ExecuteAsync( input, cancellationToken );
    }
}
