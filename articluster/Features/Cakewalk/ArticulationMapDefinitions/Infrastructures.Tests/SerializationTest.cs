using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Local;
using ArtiCluster.Shared.Mock;

using NUnit.Framework;

namespace ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.Infrastructures.Tests;

[TestFixture]
public class SerializationTest
{
    [Test]
    public void SerializeTest()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();

        var source1 = MockUniversalDefinition.CreateDefinition( id1, patchName: "Epic Lead" );
        var source2 = MockUniversalDefinition.CreateDefinition( id2, patchName: "E.Bass" );

        var productSet = new UniversalDefinitionProductSet(
            manufacturerName: source1.ManufacturerName,
            productName: source1.ProductName,
            items: [ source1, source2 ]
        );

        var mapResult = new CakewalkModelMapper().Map( productSet );

        Assert.That( mapResult.IsSuccess, Is.True, "Mapping should succeed" );

        var jsonText = JsonSerializer.Serialize( mapResult.Unwrap(), SerializationConstants.SerializerOptions );

        TestContext.Out.WriteLine( jsonText );
    }

    [Test]
    public async Task ExportTest()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();

        var source1 = MockUniversalDefinition.CreateDefinition( id1, patchName: "Epic Lead" );
        var source2 = MockUniversalDefinition.CreateDefinition( id2, patchName: "E.Bass" );

        var productSet = new UniversalDefinitionProductSet(
            manufacturerName: source1.ManufacturerName,
            productName: source1.ProductName,
            items: [ source1, source2 ]
        );

        var mapResult = new CakewalkModelMapper().Map( productSet );

        Assert.That( mapResult.IsSuccess, Is.True, "Mapping should succeed" );

        var dest = Path.GetTempFileName();


        try
        {
            await using( var fileWriter = new LocalTextContentWriter( dest ) )
            {
                var exporter = new CakewalkExporter();

                var result = await exporter.ExportAsync( fileWriter, productSet );
                Assert.That( result.IsSuccess, Is.True, $"Export should succeed." );
            }

            await TestContext.Out.WriteAsync( await File.ReadAllTextAsync( dest ) );
        }
        finally
        {
            File.Delete( dest );
        }
    }
}
