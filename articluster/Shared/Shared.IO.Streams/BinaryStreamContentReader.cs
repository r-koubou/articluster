using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Shared.IO.Stream;

public sealed class BinaryStreamContentReader( System.IO.Stream stream, int length = -1, bool leaveOpen = false ) : IBinaryContentReader
{
    // ReSharper disable MemberCanBePrivate.Global
    private System.IO.Stream Stream { get; } = stream;
    public int Length { get; } = length;
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
        if( Length >= 0 )
        {
            return await ReadFixedBytesAsync( Stream, Length, cancellationToken );
        }

        var memoryStream = new MemoryStream();
        await Stream.CopyToAsync( memoryStream, cancellationToken );

        return memoryStream.ToArray();
    }

    private static async Task<byte[]> ReadFixedBytesAsync( System.IO.Stream source, int length, CancellationToken cancellationToken = default )
    {
        if( length < 0 )
        {
            throw new ArgumentOutOfRangeException( nameof( length ), "Length must be non-negative." );
        }

        var offset = 0;
        var restBytes = length;
        var buffer = new byte[ length ];

        while( restBytes > 0 )
        {
            var readBytes = await source.ReadAsync( buffer, offset, restBytes, cancellationToken );

            if( readBytes < 0 )
            {
                break;
            }

            offset    += readBytes;
            restBytes -= readBytes;
        }

        return buffer;
    }
}
