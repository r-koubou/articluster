using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Shared.IO.Local;

public sealed class LocalBinaryContentReader( string filePath ) : IBinaryContentReader
{
    // ReSharper disable MemberCanBePrivate.Global
    public string FilePath { get; } = filePath;
    // ReSharper restore MemberCanBePrivate.Global

    public void Dispose() {}

    public async Task<byte[]> ReadContentAsync( CancellationToken cancellationToken = default )
    {
        return await File.ReadAllBytesAsync( FilePath, cancellationToken );
    }
}
