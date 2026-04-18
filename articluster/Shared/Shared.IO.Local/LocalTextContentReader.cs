using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Shared.IO.Local;

public class LocalTextContentReader( string filePath, Encoding fileEncoding ) : ITextContentReader
{
    // ReSharper disable MemberCanBePrivate.Global
    public string FilePath { get; } = filePath;
    public Encoding FileEncoding { get; } = fileEncoding;
    // ReSharper restore MemberCanBePrivate.Global

    public LocalTextContentReader( string filePath ) : this( filePath, Encoding.UTF8 ) {}

    public async Task<string> ReadContentAsync( CancellationToken cancellationToken = default )
    {
        return await File.ReadAllTextAsync( FilePath, FileEncoding, cancellationToken );
    }
}
