using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.Domain.Articulation.Model;

namespace ArtiCluster.Features.UniversalDefinition.Gateways;

public interface IDefinitionParser<in TSource>
{
    Task<Articulation> ParseAsync( TSource source, CancellationToken cancellationToken = default );
}

public interface IDefinitionStringParser : IDefinitionParser<string>;

public interface IDefinitionFileParser : IDefinitionParser<string>;

public interface IDefinitionStreamParser : IDefinitionParser<Stream>;
