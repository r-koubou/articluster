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
    public void WriteTextContentTestAsync()
    {
        var writer = new TextStreamContentWriter( File.Create( Path.GetTempFileName() ) );
        Assert.DoesNotThrowAsync( async () => await writer.WriteContentAsync( "Hello", CancellationToken.None ) );
    }

    [Test]
    public async Task WriteBinaryContentTestAsync()
    {
        var writer = new BinaryStreamContentWriter( File.Create( Path.GetTempFileName() ) );
        Assert.DoesNotThrowAsync( async () => await writer.WriteContentAsync( [ 0x01, 0x02, 0x03 ], CancellationToken.None ) );
    }
}
