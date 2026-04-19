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
) : ITextStreamContentReader
{
    // ReSharper disable MemberCanBePrivate.Global
    private Stream Stream { get; } = stream;
    private Encoding TextEncoding { get; } = textEncoding ?? Encoding.UTF8;
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

    public async Task<string> ReadContentAsync( CancellationToken cancellationToken = default )
    {
        using var binaryReader = new BinaryStreamContentReader( Stream, leaveOpen: LeaveOpen );
        var bytes = await binaryReader.ReadContentAsync( cancellationToken );

        return TextEncoding.GetString( bytes );
    }

    public async Task<string> ReadContentAsync( Count count, CancellationToken cancellationToken = default )
    {
        var buffer = ArrayPool<byte>.Shared.Rent( count.Value );

        try
        {
            var memory = new Memory<byte>( buffer, 0, count.Value );
            _ = await Stream.ReadAtLeastAsync( memory, count.Value, cancellationToken: cancellationToken );

            return TextEncoding.GetString( buffer, 0, count.Value );
        }
        finally
        {
            ArrayPool<byte>.Shared.Return( buffer );
        }
    }
}
