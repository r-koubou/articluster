using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions;
using ArtiCluster.Shared.IO.Abstractions.Values;

namespace ArtiCluster.Shared.IO.Local;

public sealed class LocalBinaryContentReader( string filePath ) : IBinaryContentReader, IDisposable, IAsyncDisposable
{
    // ReSharper disable MemberCanBePrivate.Global
    private readonly Stream fileStream = File.Open( filePath, FileMode.Open, FileAccess.Read, FileShare.Read );
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

    public async Task<ReadOnlyMemory<byte>> ReadAllAsync( CancellationToken cancellationToken = default )
    {
        using var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync( memoryStream, cancellationToken );

        return new ReadOnlyMemory<byte>( memoryStream.ToArray() );
    }

    public async Task<Count> ReadAsync( Memory<byte> buffer, Count count, CancellationToken cancellationToken = default )
    {
        var readBytes = await fileStream.ReadAtLeastAsync(
            buffer,
            count.Value,
            throwOnEndOfStream: false,
            cancellationToken: cancellationToken
        );

        return new Count( readBytes );
    }
}
