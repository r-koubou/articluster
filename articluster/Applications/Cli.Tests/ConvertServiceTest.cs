using System.IO;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services;
using ArtiCluster.Applications.Services.LocalFileConversions;
using ArtiCluster.Tests.Helpers;

using NUnit.Framework;

namespace ArtiCluster.Applications.Cli.Tests;

[TestFixture]
public class ConvertServiceTest
{
    [Test]
    public async Task BulkImportTest()
    {
        var dir = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "Acme Corp"
        );

        var service = new UniversalDefinitionLocalFileService();
        var result = await service.ImportAsync( dir );

        Assert.That( result.IsSuccess, Is.True );

        var definitions = result.Unwrap();
        Assert.That( definitions.Count, Is.EqualTo( 1 ) );
    }

    [Test]
    public async Task ExportTemplateTest()
    {
        var filePath = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "testdata.yaml"
        );

        var service = new UniversalDefinitionLocalFileService();
        var result = await service.ExportTemplateAsync( filePath );

        Assert.That( result.IsSuccess, Is.True );
    }

    [Test]
    public async Task ConvertCubaseTest()
    {
        var inputBaseDir = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "Acme Corp"
        );

        var importService = new UniversalDefinitionLocalFileService();
        var importResult = await importService.ImportAsync( inputBaseDir );

        Assert.That( importResult.IsSuccess, Is.True );

        var definitions = importResult.Unwrap();
        Assert.That( definitions.Count, Is.EqualTo( 1 ) );

        var outputBaseDir = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "converted"
        );

        var convertService = new CubaseLocalFileConversionService();
        var convertResult = await convertService.ConvertAsync( outputBaseDir, definitions );

        Assert.That( convertResult.IsSuccess, Is.True );
    }

    [Test]
    public async Task ConvertStudioOneTest()
    {
        var inputBaseDir = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "Acme Corp"
        );

        var importService = new UniversalDefinitionLocalFileService();
        var importResult = await importService.ImportAsync( inputBaseDir );

        Assert.That( importResult.IsSuccess, Is.True );

        var definitions = importResult.Unwrap();
        Assert.That( definitions.Count, Is.EqualTo( 1 ) );

        var outputBaseDir = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "converted"
        );

        var convertService = new StudioOneLocalFileConversionService();
        var convertResult = await convertService.ConvertAsync( outputBaseDir, definitions );

        Assert.That( convertResult.IsSuccess, Is.True );
    }
}
