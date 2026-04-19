using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions.Values;

namespace ArtiCluster.Shared.IO.Abstractions;

public interface ITextStreamContentReader : ITextContentReader
{
    string ReadContent( Count count )
        => ReadContentAsync( count ).GetAwaiter().GetResult();

    Task<string> ReadContentAsync( Count count, CancellationToken cancellationToken = default );
}
