using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Shared.IO.Local;

public sealed class LocalBinaryContentWriter( string filePath ) : IBinaryContentWriter, IDisposable, IAsyncDisposable
{
    // ReSharper disable MemberCanBePrivate.Global
    private readonly Stream fileStream = File.Open( filePath, FileMode.Create, FileAccess.Write );
    public string FilePath { get; } = filePath;
    // ReSharper restore MemberCanBePrivate.Global

    public void Dispose()
    {
        fileStream.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await fileStream.DisposeAsync();
    }

    public async Task WriteAsync( ReadOnlyMemory<byte> content, CancellationToken cancellationToken = default )
    {
        await fileStream.WriteAsync( content, cancellationToken );
    }
}
