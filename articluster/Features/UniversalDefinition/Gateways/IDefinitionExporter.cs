using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.Domain.Articulation.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.UniversalDefinition.Gateways;

public interface IDefinitionExporter
{
    Task ExportAsync(
        ITextContentWriter writer,
        Articulation source,
        CancellationToken cancellationToken = default
    );
}
