using System;
using System.Buffers;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons.Extensions;
using ArtiCluster.Shared.IO.Abstractions;
using ArtiCluster.Shared.IO.Abstractions.Values;

namespace ArtiCluster.Shared.IO.Streams;

public sealed class TextStreamContentReader(
    Stream stream,
    Encoding? textEncoding = null,
    bool leaveOpen = false
) : ITextStreamContentReader, IDisposable, IAsyncDisposable
{
    // ReSharper disable MemberCanBePrivate.Global
    private Stream Stream { get; } = stream;
    private Encoding TextEncoding { get; } = textEncoding ?? Encoding.UTF8;
    public bool LeaveOpen { get; } = leaveOpen;
    // ReSharper restore MemberCanBePrivate.Global

    public void Dispose()
        => DisposeAsync().GetAwaiter().GetResult();

    public async ValueTask DisposeAsync()
    {
        if( LeaveOpen )
        {
            return;
        }

        await Stream.DisposeAsync();
    }

    public async Task<string> ReadAllAsync( CancellationToken cancellationToken = default )
    {
        using var binaryReader = new BinaryStreamContentReader( Stream, leaveOpen: true );
        var buffer = await binaryReader.ReadAllAsync( cancellationToken );

        return TextEncoding.GetString( buffer.ToArray() );
    }

    public async Task<string> ReadAsync( Count count, CancellationToken cancellationToken = default )
    {
        var buffer = ArrayPool<byte>.Shared.Rent( count.Value );

        try
        {
            var memory = new Memory<byte>( buffer, 0, count.Value );
            var readByteCount = await Stream.ReadAtLeastAsync(
                memory,
                count.Value,
                throwOnEndOfStream: false,
                cancellationToken: cancellationToken
            );

            return TextEncoding.GetString( buffer, 0, readByteCount );
        }
        finally
        {
            ArrayPool<byte>.Shared.Return( buffer );
        }
    }
}
