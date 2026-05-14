using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinitions.Contracts;
using ArtiCluster.Features.UniversalDefinitions.v1.Exports;
using ArtiCluster.Features.UniversalDefinitions.v1.Imports;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.UniversalDefinitions.Facades;

public sealed class UniversalDefinitionFacade : IUniversalDefinitionFacade
{
    public async Task<Result<UniversalDefinition, ImportFailureReason>> ImportAsync( ITextContentReader reader, CancellationToken cancellationToken = default )
    {
        var importer = new YamlImporter();
        return await importer.ImportAsync( reader, cancellationToken );
    }

    public async Task<Result<Unit, ExportFailureReason>> ExportAsync( ITextContentWriter writer, UniversalDefinition source, CancellationToken cancellationToken = default )
    {
        var exporter = new YamlExporter();
        return await exporter.ExportAsync( writer, source, cancellationToken );
    }
}
