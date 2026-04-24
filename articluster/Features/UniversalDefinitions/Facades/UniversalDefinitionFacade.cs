using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinitions.Gateways;
using ArtiCluster.Features.UniversalDefinitions.Infrastructures.Yaml;
using ArtiCluster.Features.UniversalDefinitions.UseCases;
using ArtiCluster.Shared.Domain.Articulation.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.UniversalDefinitions.Facades;

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
