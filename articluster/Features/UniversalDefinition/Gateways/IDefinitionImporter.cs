using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.Domain.Articulation.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.UniversalDefinition.Gateways;

public interface IDefinitionImporter
{
    Task<Articulation> ImportAsync(
        ITextContentReader reader,
        CancellationToken cancellationToken = default
    );
}
