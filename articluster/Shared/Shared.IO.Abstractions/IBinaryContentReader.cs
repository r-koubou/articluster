using System;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions.Values;

namespace ArtiCluster.Shared.IO.Abstractions;

public interface IBinaryContentReader : IContentReader<byte[]>
{
    Count ReadContent( Memory<byte> buffer, Count count )
        => ReadContentAsync( buffer, count ).GetAwaiter().GetResult();

    Task<Count> ReadContentAsync( Memory<byte> buffer, Count count, CancellationToken cancellationToken = default );
}
