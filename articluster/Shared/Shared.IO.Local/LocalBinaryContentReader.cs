using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons.Extensions;
using ArtiCluster.Shared.IO.Abstractions;
using ArtiCluster.Shared.IO.Abstractions.Values;

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

    public async Task<Count> ReadContentAsync( Memory<byte> buffer, Count count, CancellationToken cancellationToken = default )
    {
        await using var fileStream = new FileStream( FilePath, FileMode.Open, FileAccess.Read, FileShare.Read, buffer.Length, useAsync: true );
        var readBytes = await fileStream.ReadAtLeastAsync( buffer, count.Value, cancellationToken: cancellationToken );

        return new Count( readBytes );
    }
}
