using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Shared.IO.Streams;

public sealed class TextStreamContentReader(
    Stream stream,
    Encoding? textEncoding = null,
    bool leaveOpen = false
) : ITextContentReader
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
}
