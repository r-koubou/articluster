using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Shared.IO.Streams;

public sealed class BinaryStreamContentWriter( Stream stream, bool leaveOpen = false ) : IBinaryContentWriter
{
    // ReSharper disable MemberCanBePrivate.Global
    private Stream Stream { get; } = stream;
    public bool LeaveOpen { get; } = leaveOpen;
    // ReSharper restore MemberCanBePrivate.Global

    public void Dispose()
    {
        if( LeaveOpen )
        {
            return;
        }

        Stream.Dispose();
    }

    public async Task WriteContentAsync( byte[] content, CancellationToken cancellationToken = default )
    {
        await Stream.WriteAsync( content, cancellationToken );
    }

    public async Task WriteContentAsync( byte[] value, int offset, int count, CancellationToken cancellationToken = default )
    {
        await Stream.WriteAsync( value.AsMemory( offset, count ), cancellationToken );
    }
}
