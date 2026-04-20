using System;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions.Values;

namespace ArtiCluster.Shared.IO.Abstractions;

public interface IBinaryContentReader : IContentReader<ReadOnlyMemory<byte>>
{
    Task<Count> ReadAsync( Memory<byte> buffer, Count count, CancellationToken cancellationToken = default );
}
