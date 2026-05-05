using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Tests.Helpers;

using NUnit.Framework;

namespace ArtiCluster.Shared.IO.Local.Tests.Local;

[TestFixture]
public class LocalFileReaderTest
{
    [Test]
    public async Task ReadTextContentTestAsync()
    {
        using var reader = new LocalTextContentReader( Path.Combine( TestUtility.TestDataDirectoryRoot, "reader-text.txt" ) );
        var content = await reader.ReadAllAsync( CancellationToken.None );

        Assert.That( content, Is.EqualTo( "Hello" ) );
    }

    [Test]
    public async Task ReadBinaryContentTestAsync()
    {
        await using var reader = new LocalBinaryContentReader( Path.Combine( TestUtility.TestDataDirectoryRoot, "reader-binary.bin" ) );
        var content = await reader.ReadAllAsync( CancellationToken.None );

        Assert.That( content.ToArray(), Is.EqualTo( new byte[] { 0x01, 0x02, 0x03 } ) );
    }
}
