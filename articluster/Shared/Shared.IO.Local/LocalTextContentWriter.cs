using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Shared.IO.Local;

public sealed class LocalTextContentWriter( string filePath, Encoding textEncoding ) : ITextContentWriter
{
    // ReSharper disable MemberCanBePrivate.Global
    public string FilePath { get; } = filePath;
    public Encoding TextEncoding { get; } = textEncoding;
    // ReSharper restore MemberCanBePrivate.Global

    public void Dispose() {}

    public LocalTextContentWriter( string filePath ) : this( filePath, Encoding.UTF8 ) {}

    public async Task WriteContentAsync( string content, CancellationToken cancellationToken = default )
    {
        await File.WriteAllTextAsync( FilePath, content, TextEncoding, cancellationToken );
    }
}
