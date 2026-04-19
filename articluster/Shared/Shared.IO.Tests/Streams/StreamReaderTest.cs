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
        var reader = new TextStreamContentReader( File.OpenRead( Path.Combine( Constants.TestDataDirectoryRoot, "reader-text.txt" ) ) );
        var content = await reader.ReadContentAsync( CancellationToken.None );

        Assert.AreEqual( "Hello", content );
    }

    [Test]
    public async Task ReadPartialTextContentTestAsync()
    {
        var reader = new TextStreamContentReader( File.OpenRead( Path.Combine( Constants.TestDataDirectoryRoot, "reader-text.txt" ) ) );
        var content = await reader.ReadContentAsync( new Count( 3 ), CancellationToken.None );

        Assert.AreEqual( "Hel", content );
    }

    [Test]
    public async Task ReadBinaryContentTestAsync()
    {
        var reader = new BinaryStreamContentReader( File.OpenRead( Path.Combine( Constants.TestDataDirectoryRoot, "reader-binary.bin" ) ) );
        var content = await reader.ReadContentAsync( CancellationToken.None );

        Assert.AreEqual( new byte[] { 0x01, 0x02, 0x03 }, content );
    }

    [Test]
    public async Task ReadPartialBinaryContentTestAsync()
    {
        var buffer = new Memory<byte>( new byte[ 2 ] );
        var reader = new BinaryStreamContentReader( File.OpenRead( Path.Combine( Constants.TestDataDirectoryRoot, "reader-binary.bin" ) ) );
        var readBytes = await reader.ReadContentAsync( buffer, new Count( 2 ), CancellationToken.None );

        Assert.AreEqual( new Count( 2 ), readBytes );
        Assert.AreEqual( new byte[] { 0x01, 0x02 }, buffer.ToArray() );
    }
}
