using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions.Values;

namespace ArtiCluster.Shared.IO.Abstractions;

public interface ITextStreamContentReader : ITextContentReader
{
    Task<string> ReadAsync( Count count, CancellationToken cancellationToken = default );
}
