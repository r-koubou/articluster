using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Shared.IO.Streams;

public sealed class TextStreamContentWriter( Stream stream, Encoding? encoding = null, bool leaveOpen = false ) : ITextContentWriter
{
    // ReSharper disable MemberCanBePrivate.Global
    private Stream Stream { get; } = stream;
    public Encoding TextEncoding { get; } = encoding ?? Encoding.UTF8;
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

    public async Task WriteContentAsync( string content, CancellationToken cancellationToken )
    {
        await using var writer = new StreamWriter( Stream, encoding: TextEncoding, bufferSize: -1, leaveOpen: true );
        await writer.WriteAsync( content );
    }
}
