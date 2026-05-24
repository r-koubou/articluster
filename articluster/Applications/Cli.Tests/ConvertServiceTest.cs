using System.IO;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Executors;
using ArtiCluster.Applications.Services.Local;
using ArtiCluster.Applications.Services.Local.Executors;
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

        var executor = new UniversalDefinitionImportExecutor( new NullLogger<IUniversalDefinitionImportExecutor>() );

        var service = new UniversalDefinitionFileService(
            new NullLogger<UniversalDefinitionFileService>(),
            executor
        );

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

        var importExecutor = new UniversalDefinitionImportExecutor( new NullLogger<IUniversalDefinitionImportExecutor>() );

        var service = new UniversalDefinitionFileService(
            new NullLogger<UniversalDefinitionFileService>(),
            importExecutor
        );

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

        var importExecutor = new UniversalDefinitionImportExecutor( new NullLogger<IUniversalDefinitionImportExecutor>() );

        var importService = new UniversalDefinitionFileService(
            new NullLogger<UniversalDefinitionFileService>(),
            importExecutor
        );

        var importResult = await importService.ImportAsync( inputBaseDir );

        Assert.That( importResult.IsSuccess, Is.True );

        var definitions = importResult.Unwrap();
        Assert.That( definitions.Count, Is.EqualTo( 1 ) );

        var outputBaseDir = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "converted"
        );

        var outputMarkdownDir = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "markdown"
        );

        var convertService = new CakewalkExportFileService( new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory() );
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

        var importExecutor = new UniversalDefinitionImportExecutor( new NullLogger<IUniversalDefinitionImportExecutor>() );

        var importService = new UniversalDefinitionFileService(
            new NullLogger<UniversalDefinitionFileService>(),
            importExecutor
        );

        var importResult = await importService.ImportAsync( inputBaseDir );

        Assert.That( importResult.IsSuccess, Is.True );

        var definitions = importResult.Unwrap();
        Assert.That( definitions.Count, Is.EqualTo( 1 ) );

        var outputBaseDir = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "converted"
        );

        var outputMarkdownDir = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "markdown"
        );

        var convertService = new CubaseExportFileService( new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory() );
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

        var importExecutor = new UniversalDefinitionImportExecutor( new NullLogger<IUniversalDefinitionImportExecutor>() );

        var importService = new UniversalDefinitionFileService(
            new NullLogger<UniversalDefinitionFileService>(),
            importExecutor
        );

        var importResult = await importService.ImportAsync( inputBaseDir );

        Assert.That( importResult.IsSuccess, Is.True );

        var definitions = importResult.Unwrap();
        Assert.That( definitions.Count, Is.EqualTo( 1 ) );

        var outputBaseDir = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "converted"
        );

        var outputMarkdownDir = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "markdown"
        );

        var convertService = new LogicExportFileService( new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory() );
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

        var importExecutor = new UniversalDefinitionImportExecutor( new NullLogger<IUniversalDefinitionImportExecutor>() );

        var importService = new UniversalDefinitionFileService(
            new NullLogger<UniversalDefinitionFileService>(),
            importExecutor
        );

        var importResult = await importService.ImportAsync( inputBaseDir );

        Assert.That( importResult.IsSuccess, Is.True );

        var definitions = importResult.Unwrap();
        Assert.That( definitions.Count, Is.EqualTo( 1 ) );

        var outputBaseDir = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "converted"
        );

        var outputMarkdownDir = Path.Combine(
            TestUtility.TestDataDirectoryRoot,
            "markdown"
        );

        var convertService = new StudioOneExportFileService( new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory() );
        var convertResult = await convertService.ExportAsync( outputBaseDir, definitions );

        Assert.That( convertResult.IsSuccess, Is.True );
    }
}
