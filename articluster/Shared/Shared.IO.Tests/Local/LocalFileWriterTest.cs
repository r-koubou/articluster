using System.IO;
using System.Threading;

using NUnit.Framework;

namespace ArtiCluster.Shared.IO.Local.Tests.Local;

[TestFixture]
public class LocalFileWriterReaderTest
{
    [Test]
    public void WriteTextContentTest()
    {
        var writer = new LocalTextContentWriter( Path.GetTempFileName() );
        Assert.DoesNotThrowAsync( async () => await writer.WriteContentAsync( "Hello", CancellationToken.None ) );
    }

    [Test]
    public void WriteBinaryContentTest()
    {
        var writer = new LocalBinaryContentWriter( Path.GetTempFileName() );
        Assert.DoesNotThrowAsync( async () => await writer.WriteContentAsync( [ 0x01, 0x02, 0x03 ], CancellationToken.None ) );
    }
}
