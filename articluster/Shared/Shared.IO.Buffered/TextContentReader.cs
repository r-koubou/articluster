using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Shared.IO.Buffered;

public sealed class TextContentReader( string text )
    : ITextContentReader, IDisposable
{
    // ReSharper disable MemberCanBePrivate.Global
    private StringReader Stream { get; } = new StringReader( text );
    // ReSharper restore MemberCanBePrivate.Global

    public void Dispose()
    {
        Stream.Dispose();
    }

    public async Task<string> ReadAllAsync( CancellationToken cancellationToken = default )
    {
        return await Stream.ReadToEndAsync( cancellationToken );
    }
}
