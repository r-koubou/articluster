using System;
using System.IO;
using System.Threading.Tasks;

using ArtiCluster.Shared.IO.Local;

using NUnit.Framework;

namespace ArtiCluster.Features.Logic.ArticulationSetDefinitions.Infrastructures.Tests;

[TestFixture]
public class SerializationTest
{
    [Test]
    public async Task ExportTest()
    {
        var id = Guid.NewGuid();
        var source = TestUtility.CreateMock( id, patchName: "Epic Lead" );

        var dest = Path.GetTempFileName();

        try
        {
            await using( var fileWriter = new LocalTextContentWriter( dest ) )
            {
                var exporter = new LogicExporter();
                var result = await exporter.ExportAsync( fileWriter, source );
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
