using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using ArtiCluster.Features.Logic.Articulations.Exports;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Local;
using ArtiCluster.Shared.Mock;

using NUnit.Framework;

namespace ArtiCluster.Features.Logic.Articulations.Tests;

[TestFixture]
public class SerializationTest
{
    [Test]
    public async Task ExportTest()
    {
        var id = Guid.NewGuid();
        var mock = MockUniversalDefinition.CreateDefinition( id, patchName: "Epic Lead" );

        var source = SeparatedArticulationGroupSet.Create(
            mock.ManufacturerName.Value,
            mock.ProductName.Value,
            mock.PatchName.Value,
            "Main",
            mock.ArticulationGroups.Single().Articulations
        );

        var dest = Path.GetTempFileName();

        try
        {
            await using( var fileWriter = new LocalTextContentWriter( dest ) )
            {
                var exporter = new LogicExporter();
                var result = await exporter.ExportAsync( fileWriter, source );
                Assert.That( result.IsSuccess, Is.True, $"Export should succeed." );
            }


            var exportedText = await File.ReadAllTextAsync( dest );

            Assert.Multiple( () =>
                {
                    Assert.That( exportedText, Is.Not.Null.And.Not.Empty, "Exported text should not be empty." );
                    Assert.That( exportedText, Does.Contain( "<plist" ), "Export should produce plist/XML content." );
                    Assert.That( exportedText, Does.Contain( "</plist>" ), "Export should close the plist document." );
                    Assert.That( exportedText, Does.Contain( "Epic Lead" ), "Export should contain the patch name." );
                }
            );

            await TestContext.Out.WriteAsync( exportedText );
        }
        finally
        {
            File.Delete( dest );
        }
    }
}
