using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Features.UniversalDefinitions.Gateways;
using ArtiCluster.Shared.IO.Streams;

using NUnit.Framework;

namespace ArtiCluster.Features.UniversalDefinitions.Infrastructures.Yaml.Tests;

[TestFixture]
public class YamlImportTest
{
    [Test]
    public async Task LoadOkTestAsync()
    {
        using var stream = File.OpenRead( Constants.MakeTestDataPath( "ok.yaml" ) );
        using var reader = new TextStreamContentReader( stream );
        var importer = new YamlImporter();
        var result = await importer.ImportAsync( reader, CancellationToken.None );

        Assert.That( result.IsSuccess, Is.True, "Import should succeed" );
    }

    [Test]
    public async Task DeserializationFailureTestAsync()
    {
        using var stream = File.OpenRead( Constants.MakeTestDataPath( "invalid.yaml" ) );
        using var reader = new TextStreamContentReader( stream );
        var importer = new YamlImporter();
        var result = await importer.ImportAsync( reader, CancellationToken.None );

        Assert.That( result.IsFailure, Is.True, "Import should fail" );
        Assert.That( result.Reason, Is.EqualTo( ImportReason.DeserializationError ), "Invalid yaml spec" );
    }
}
