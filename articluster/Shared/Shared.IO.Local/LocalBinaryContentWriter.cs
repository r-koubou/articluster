using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Shared.IO.Local;

public sealed class LocalBinaryContentWriter( string filePath ) : IBinaryContentWriter
{
    // ReSharper disable MemberCanBePrivate.Global
    public string FilePath { get; } = filePath;
    // ReSharper restore MemberCanBePrivate.Global

    public void Dispose() {}

    public async Task WriteContentAsync( byte[] content, CancellationToken cancellationToken = default )
    {
        await File.WriteAllBytesAsync( FilePath, content, cancellationToken );
    }

    public async Task WriteContentAsync( byte[] value, int offset, int count, CancellationToken cancellationToken = default )
    {
        await using var fileStream = File.Open( FilePath, FileMode.OpenOrCreate, FileAccess.Write );
        await fileStream.WriteAsync( value, offset, count, cancellationToken );
    }
}
