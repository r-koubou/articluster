using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

namespace ArtiCluster.Shared.IO.Local.Tests.Local;

[TestFixture]
public class LocalFileWriterTest
{
    [Test]
    public async Task WriteTextContentTestAsync()
    {
        var path = Path.GetTempFileName();

        try
        {
            // Write
            {
                using var writer = new LocalTextContentWriter( path );
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
        var testData = new ReadOnlyMemory<byte>( [ 0x01, 0x02, 0x03 ] );
        var path = Path.GetTempFileName();

        try
        {
            // Write
            {
                using var writer = new LocalBinaryContentWriter( path );
                await writer.WriteAsync( testData, CancellationToken.None );
            }
            // Verify
            {
                var binary = await File.ReadAllBytesAsync( path );
                Assert.That( binary, Is.EqualTo( testData.ToArray() ) );
            }
        }
        finally
        {
            File.Delete( path );
        }
    }
}
