using System.IO;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

namespace ArtiCluster.Shared.IO.Local.Tests;

[TestFixture]
public class LocalFileReaderTest
{
    [Test]
    public async Task ReadTextContentTestAsync()
    {
        var reader = new LocalTextContentReader( Path.Combine( Constants.TestDataDirectoryRoot, "reader-text.txt" ) );
        var content = await reader.ReadContentAsync( CancellationToken.None );

        Assert.AreEqual( "Hello", content );
    }

    [Test]
    public async Task ReadBinaryContentTestAsync()
    {
        var reader = new LocalBinaryContentReader( Path.Combine( Constants.TestDataDirectoryRoot, "reader-binary.bin" ) );
        var content = await reader.ReadContentAsync( CancellationToken.None );

        Assert.AreEqual( new byte[] { 0x01, 0x02, 0x03 }, content );
    }
}
