using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Features.UniversalDefinitions.Contracts;
using ArtiCluster.Features.UniversalDefinitions.Imports;
using ArtiCluster.Shared.IO.Streams;
using ArtiCluster.Tests.Helpers;

using NUnit.Framework;

namespace ArtiCluster.Features.UniversalDefinitions.Tests;

[TestFixture]
public class YamlImportTest
{
    [Test]
    public async Task LoadOkTestAsync()
    {
        await using var stream = File.OpenRead( TestUtility.MakeTestDataPath( "ok.yaml" ) );
        await using var reader = new TextStreamContentReader( stream );
        var importer = new YamlImporter();
        var result = await importer.ImportAsync( reader, CancellationToken.None );

        Assert.That( result.IsSuccess, Is.True, "Import should succeed" );
    }

    [Test]
    public async Task DeserializationFailureTestAsync()
    {
        await using var stream = File.OpenRead( TestUtility.MakeTestDataPath( "invalid.yaml" ) );
        await using var reader = new TextStreamContentReader( stream );
        var importer = new YamlImporter();
        var result = await importer.ImportAsync( reader, CancellationToken.None );

        Assert.That( result.IsFailure, Is.True, "Import should fail" );
        Assert.That( result.Reason, Is.EqualTo( ImportFailureReason.DeserializationError ), "Invalid yaml spec" );
    }
}
