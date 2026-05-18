using System.IO;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Local;
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

        var service = new UniversalDefinitionFileService( new NullLogger<UniversalDefinitionFileService>() );
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

        var service = new UniversalDefinitionFileService( new NullLogger<UniversalDefinitionFileService>() );
        var result = await service.ExportTemplateAsync( filePath );

        Assert.That( result.IsSuccess, Is.True );
    }
    [Test]
    public async Task ConvertCakewalkTest()
    {
        var inputBaseDir = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "Acme Corp"
        );

        var importService = new UniversalDefinitionFileService( new NullLogger<UniversalDefinitionFileService>() );
        var importResult = await importService.ImportAsync( inputBaseDir );

        Assert.That( importResult.IsSuccess, Is.True );

        var definitions = importResult.Unwrap();
        Assert.That( definitions.Count, Is.EqualTo( 1 ) );

        var outputBaseDir = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "converted"
        );

        var convertService = new CakewalkOutputFileService( new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory() );
        var convertResult = await convertService.ExportAsync( outputBaseDir, definitions );

        Assert.That( convertResult.IsSuccess, Is.True );
    }

    [Test]
    public async Task ConvertCubaseTest()
    {
        var inputBaseDir = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "Acme Corp"
        );

        var importService = new UniversalDefinitionFileService( new NullLogger<UniversalDefinitionFileService>() );
        var importResult = await importService.ImportAsync( inputBaseDir );

        Assert.That( importResult.IsSuccess, Is.True );

        var definitions = importResult.Unwrap();
        Assert.That( definitions.Count, Is.EqualTo( 1 ) );

        var outputBaseDir = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "converted"
        );

        var convertService = new CubaseOutputFileService( new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory() );
        var convertResult = await convertService.ExportAsync( outputBaseDir, definitions );

        Assert.That( convertResult.IsSuccess, Is.True );
    }

    [Test]
    public async Task ConvertLogicTest()
    {
        var inputBaseDir = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "Acme Corp"
        );

        var importService = new UniversalDefinitionFileService( new NullLogger<UniversalDefinitionFileService>() );
        var importResult = await importService.ImportAsync( inputBaseDir );

        Assert.That( importResult.IsSuccess, Is.True );

        var definitions = importResult.Unwrap();
        Assert.That( definitions.Count, Is.EqualTo( 1 ) );

        var outputBaseDir = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "converted"
        );

        var convertService = new LogicOutputFileService( new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory() );
        var convertResult = await convertService.ExportAsync( outputBaseDir, definitions );

        Assert.That( convertResult.IsSuccess, Is.True );
    }

    [Test]
    public async Task ConvertStudioOneTest()
    {
        var inputBaseDir = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "Acme Corp"
        );

        var importService = new UniversalDefinitionFileService( new NullLogger<UniversalDefinitionFileService>() );
        var importResult = await importService.ImportAsync( inputBaseDir );

        Assert.That( importResult.IsSuccess, Is.True );

        var definitions = importResult.Unwrap();
        Assert.That( definitions.Count, Is.EqualTo( 1 ) );

        var outputBaseDir = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "converted"
        );

        var convertService = new StudioOneOutputFileService( new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory() );
        var convertResult = await convertService.ExportAsync( outputBaseDir, definitions );

        Assert.That( convertResult.IsSuccess, Is.True );
    }
}
