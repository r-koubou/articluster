using System;
using System.Threading;
using System.Threading.Tasks;

namespace ArtiCluster.Shared.IO.Abstractions;

public interface IBinaryContentWriter : IContentWriter<byte[]>
{
    void WriteContent( ReadOnlyMemory<byte> content )
        => WriteContentAsync( content ).GetAwaiter().GetResult();

    Task WriteContentAsync( ReadOnlyMemory<byte> content, CancellationToken cancellationToken = default );
}
