using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions;
using ArtiCluster.Shared.IO.Stream.ValueObjects;

namespace ArtiCluster.Shared.IO.Stream;

public sealed class TextStreamContentReader(
    System.IO.Stream stream,
    ReadLength? length = null,
    Encoding? textEncoding = null,
    bool leaveOpen = false
) : ITextContentReader
{
    // ReSharper disable MemberCanBePrivate.Global
    private System.IO.Stream Stream { get; } = stream;
    private Encoding TextEncoding { get; } = textEncoding ?? Encoding.UTF8;
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

    public async Task<string> ReadContentAsync( CancellationToken cancellationToken = default )
    {
        using var binaryReader = new BinaryStreamContentReader( Stream, length: Length, leaveOpen: LeaveOpen );
        var bytes = await binaryReader.ReadContentAsync( cancellationToken );

        return TextEncoding.GetString( bytes );
    }
}
