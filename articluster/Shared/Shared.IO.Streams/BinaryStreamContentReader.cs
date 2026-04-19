using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons.Extensions;
using ArtiCluster.Shared.IO.Abstractions;
using ArtiCluster.Shared.IO.Abstractions.Values;

namespace ArtiCluster.Shared.IO.Streams;

public sealed class BinaryStreamContentReader( Stream stream, bool leaveOpen = false ) : IBinaryContentReader
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

    public async Task<byte[]> ReadContentAsync( CancellationToken cancellationToken = default )
    {
        using var memoryStream = new MemoryStream();
        await Stream.CopyToAsync( memoryStream, cancellationToken );

        return memoryStream.ToArray();
    }

    public async Task<Count> ReadContentAsync( Memory<byte> buffer, Count count, CancellationToken cancellationToken = default )
    {
        var readBytes = await Stream.ReadAtLeastAsync( buffer, count.Value, cancellationToken: cancellationToken );
        return new Count( readBytes );
    }
}
