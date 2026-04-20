using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Shared.IO.Local;

public sealed class LocalTextContentReader(
    string filePath,
    Encoding? textEncoding
) : ITextContentReader, IDisposable
{
    // ReSharper disable MemberCanBePrivate.Global
    private readonly StreamReader streamReader = new(
        File.Open( filePath, FileMode.Open, FileAccess.Read ),
        encoding: textEncoding ?? Encoding.UTF8
    );

    public string FilePath { get; } = filePath;
    public Encoding TextEncoding { get; } = textEncoding  ?? Encoding.UTF8;
    // ReSharper restore MemberCanBePrivate.Global

    public void Dispose()
    {
        streamReader.Dispose();
    }

    public LocalTextContentReader( string filePath ) : this( filePath, Encoding.UTF8 ) {}

    public async Task<string> ReadAllAsync( CancellationToken cancellationToken = default )
    {
        return await streamReader.ReadToEndAsync();
    }
}
