using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinitions.Gateways;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.UniversalDefinitions.UseCases;

public sealed class ImportInputPort
{
    public IDefinitionImporter Importer { get; init; }
    public ITextContentReader ContentReader { get; init; }

    // ReSharper disable once ConvertToPrimaryConstructor
    public ImportInputPort( IDefinitionImporter importer, ITextContentReader contentReader )
    {
        Importer      = importer;
        ContentReader = contentReader;
    }
}

public sealed class ImportUseCase
{
    // ReSharper disable once MemberCanBeMadeStatic.Global
    public async Task<Result<UniversalDefinition, ImportReason>> ExecuteAsync( ImportInputPort input, CancellationToken cancellationToken = default )
    {
        return await input.Importer.ImportAsync( input.ContentReader, cancellationToken );
    }
}
