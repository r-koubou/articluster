using System.Threading;
using System.Threading.Tasks;

namespace ArtiCluster.Shared.IO.Abstractions;

public interface IBinaryContentWriter : IContentWriter<byte[]>
{
    void WriteContent( byte[] value, int offset, int count )
        => WriteContentAsync( value, offset, count ).GetAwaiter().GetResult();

    Task WriteContentAsync( byte[] value, int offset, int count, CancellationToken cancellationToken = default );
}
