using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Shared.IO.Local;

public class LocalBinaryContentWriter( string filePath ) : IBinaryContentWriter
{
    // ReSharper disable MemberCanBePrivate.Global
    public string FilePath { get; } = filePath;
    // ReSharper restore MemberCanBePrivate.Global

    public async Task WriteContentAsync( byte[] content, CancellationToken cancellationToken = default )
    {
        await File.WriteAllBytesAsync( FilePath, content, cancellationToken );
    }
}
