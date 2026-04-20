using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Abstractions.Values;
using ArtiCluster.Shared.IO.Streams;

using NUnit.Framework;

namespace ArtiCluster.Shared.IO.Local.Tests.Streams;

[TestFixture]
public class StreamReaderTest
{
    [Test]
    public async Task ReadTextContentTestAsync()
    {
        using var reader = new TextStreamContentReader( File.OpenRead( Path.Combine( Constants.TestDataDirectoryRoot, "reader-text.txt" ) ) );
        var content = await reader.ReadAllAsync( CancellationToken.None );

        Assert.That( content, Is.EqualTo( "Hello" ) );
    }

    [Test]
    public async Task ReadPartialTextContentTestAsync()
    {
        using var reader = new TextStreamContentReader( File.OpenRead( Path.Combine( Constants.TestDataDirectoryRoot, "reader-text.txt" ) ) );
        var content = await reader.ReadAsync( new Count( 3 ), CancellationToken.None );

        Assert.That( content, Is.EqualTo( "Hel" ) );
    }

    [Test]
    public async Task ReadBinaryContentTestAsync()
    {
        using var reader = new BinaryStreamContentReader( File.OpenRead( Path.Combine( Constants.TestDataDirectoryRoot, "reader-binary.bin" ) ) );
        var content = await reader.ReadAllAsync( CancellationToken.None );

        Assert.That( content.ToArray(), Is.EqualTo( new byte[] { 0x01, 0x02, 0x03 } ) );
    }

    [Test]
    public async Task ReadPartialBinaryContentTestAsync()
    {
        var buffer = new Memory<byte>( new byte[ 2 ] );
        using var reader = new BinaryStreamContentReader( File.OpenRead( Path.Combine( Constants.TestDataDirectoryRoot, "reader-binary.bin" ) ) );
        var readBytes = await reader.ReadAsync( buffer, new Count( 2 ), CancellationToken.None );

        Assert.That( readBytes, Is.EqualTo( new Count( 2 ) ) );
        Assert.That( buffer[ ..readBytes.Value ].ToArray(), Is.EqualTo( new byte[] { 0x01, 0x02 } ) );
    }
}
