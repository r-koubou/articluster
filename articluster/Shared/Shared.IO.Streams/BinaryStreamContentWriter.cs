using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Shared.IO.Streams;

public sealed class BinaryStreamContentWriter( Stream stream, bool leaveOpen = false )
    : IBinaryContentWriter, IDisposable, IAsyncDisposable
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

    public async ValueTask DisposeAsync()
    {
        if( LeaveOpen )
        {
            return;
        }

        await Stream.DisposeAsync();
    }

    public async Task WriteAsync( ReadOnlyMemory<byte> content, CancellationToken cancellationToken = default )
    {
        await Stream.WriteAsync( content, cancellationToken );
    }
}
