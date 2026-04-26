using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinitions.Gateways;
using ArtiCluster.Features.UniversalDefinitions.Infrastructures.Yaml;
using ArtiCluster.Features.UniversalDefinitions.UseCases;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.UniversalDefinitions.Facades;

public sealed class UniversalDefinitionFacade : IUniversalDefinitionFacade
{
    public async Task<Result<UniversalDefinition, ImportReason>> ImportAsync( ITextContentReader reader, CancellationToken cancellationToken = default )
    {
        var importer = new YamlImporter();
        var useCase = new ImportUseCase();
        var input = new ImportInputPort( importer, reader );

        return await useCase.ExecuteAsync( input, cancellationToken );
    }

    public async Task<Result<Unit, ExportReason>> ExportAsync( ITextContentWriter writer, UniversalDefinition source, CancellationToken cancellationToken = default )
    {
        var exporter = new YamlExporter();
        var useCase = new ExportUseCase();
        var input = new ExportInputPort( source, exporter, writer );

        return await useCase.ExecuteAsync( input, cancellationToken );
    }
}
