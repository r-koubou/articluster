using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ArtiCluster.Commons.Extensions;

public static class StreamExtension
{
    /// <summary>
    /// Port for .NET Standard
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/ja-jp/dotnet/api/system.io.stream.readatleastasync"/>
    public static async Task<int> ReadAtLeastAsync( this Stream stream, Memory<byte> buffer, int minimumBytes, bool throwOnEndOfStream = true, CancellationToken cancellationToken = default )
    {
        if( buffer.Length < minimumBytes )
        {
            throw new ArgumentException( $"The buffer length must be at least {minimumBytes}.", nameof( buffer ) );
        }

        var totalRead = 0;

        while( totalRead < minimumBytes )
        {
            var read = await stream.ReadAsync( buffer[ totalRead.. ], cancellationToken );

            if( read == 0 )
            {
                return throwOnEndOfStream
                    ? throw new EndOfStreamException()
                    : totalRead;
            }

            totalRead += read;
        }

        return totalRead;
    }
}
