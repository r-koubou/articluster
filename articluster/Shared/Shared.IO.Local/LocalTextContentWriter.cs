using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Shared.IO.Local;

public sealed class LocalTextContentWriter(
    string filePath,
    Encoding? textEncoding = null
) : ITextContentWriter, IDisposable, IAsyncDisposable
{
    // ReSharper disable MemberCanBePrivate.Global
    private readonly StreamWriter streamWriter = new(
        File.Open( filePath,
                   FileMode.Create,
                   FileAccess.Write
        ),
        encoding: textEncoding ?? Encoding.UTF8
    );

    public string FilePath { get; } = filePath;
    public Encoding TextEncoding { get; } = textEncoding ?? Encoding.UTF8;
    // ReSharper restore MemberCanBePrivate.Global

    public LocalTextContentWriter( string filePath ) : this( filePath, Encoding.UTF8 ) {}

    public void Dispose()
    {
        streamWriter.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await streamWriter.DisposeAsync();
    }

    public async Task WriteAsync( string content, CancellationToken cancellationToken = default )
    {
        await streamWriter.WriteAsync( content.AsMemory(), cancellationToken );
    }
}
