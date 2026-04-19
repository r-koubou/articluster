using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Shared.IO.Local;

public sealed class LocalTextContentReader( string filePath, Encoding textEncoding ) : ITextContentReader
{
    // ReSharper disable MemberCanBePrivate.Global
    public string FilePath { get; } = filePath;
    public Encoding TextEncoding { get; } = textEncoding;
    // ReSharper restore MemberCanBePrivate.Global

    public void Dispose() {}

    public LocalTextContentReader( string filePath ) : this( filePath, Encoding.UTF8 ) {}

    public async Task<string> ReadContentAsync( CancellationToken cancellationToken = default )
    {
        return await File.ReadAllTextAsync( FilePath, TextEncoding, cancellationToken );
    }
}
