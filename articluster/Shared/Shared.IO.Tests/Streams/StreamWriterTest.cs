using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Streams;

using NUnit.Framework;

namespace ArtiCluster.Shared.IO.Local.Tests.Streams;

[TestFixture]
public class StreamWriterTest
{
    [Test]
    public async Task WriteTextContentTestAsync()
    {
        var path = Path.GetTempFileName();

        try
        {
            // Write
            {
                await using var writer = new TextStreamContentWriter( File.Create( path ) );
                await writer.WriteAsync( "Hello", CancellationToken.None );
            }
            // Verify
            {
                var text = await File.ReadAllTextAsync( path );
                Assert.That( text, Is.EqualTo( "Hello" ) );
            }
        }
        finally
        {
            File.Delete( path );
        }
    }

    [Test]
    public async Task WriteBinaryContentTestAsync()
    {
        var path = Path.GetTempFileName();
        var testData = new ReadOnlyMemory<byte>( [ 0x01, 0x02, 0x03 ] );

        try
        {
            // Write
            {
                await using var writer = new BinaryStreamContentWriter( File.Create( path ) );
                await writer.WriteAsync( testData, CancellationToken.None );
            }
            // Verify
            {
                var bytes = await File.ReadAllBytesAsync( path );
                Assert.That( bytes, Is.EqualTo( testData.ToArray() ) );
            }
        }
        finally
        {
            File.Delete( path );
        }
    }
}
