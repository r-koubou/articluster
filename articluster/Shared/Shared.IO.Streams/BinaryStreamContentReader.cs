using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions;
using ArtiCluster.Shared.IO.Stream.ValueObjects;

namespace ArtiCluster.Shared.IO.Stream;

public sealed class BinaryStreamContentReader(
    System.IO.Stream stream,
    ReadLength? length = null,
    bool leaveOpen = false
) : IBinaryContentReader
{
    // ReSharper disable MemberCanBePrivate.Global
    private System.IO.Stream Stream { get; } = stream;
    public ReadLength Length { get; } = length ?? ReadLength.ToEnd;
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
        if( Length != ReadLength.ToEnd )
        {
            return await ReadFixedBytesAsync( Stream, Length, cancellationToken );
        }

        var memoryStream = new MemoryStream();
        await Stream.CopyToAsync( memoryStream, cancellationToken );

        return memoryStream.ToArray();
    }

    private static async Task<byte[]> ReadFixedBytesAsync( System.IO.Stream source, ReadLength length, CancellationToken cancellationToken = default )
    {
        var offset = 0;
        var restBytes = length.Value;
        var buffer = new byte[ length.Value ];

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
