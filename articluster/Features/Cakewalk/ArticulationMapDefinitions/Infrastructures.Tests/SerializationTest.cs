using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.Gateways;
using ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.Infrastructures.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;
using ArtiCluster.Shared.IO.Local;

using NUnit.Framework;

namespace ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.Infrastructures.Tests;

[TestFixture]
public class SerializationTest
{
    [Test]
    public void SerializeTest()
    {
        var id = Guid.NewGuid();
        var source = TestUtility.CreateMock( id, patchName: "Epic Lead" );

        var mapResult = new CakewalkModelMapper().Map( source );

        Assert.That( mapResult.IsSuccess, Is.True, "Mapping should succeed" );

        var jsonText = JsonSerializer.Serialize<CakewalkArticulationMap>( mapResult.Unwrap(), SerializationConstants.SerializerOptions );

        TestContext.Out.WriteLine( jsonText );
    }

    [Test]
    public async Task ExportTest()
    {
        var id = Guid.NewGuid();
        var source = TestUtility.CreateMock( id, patchName: "Epic Lead" );

        var mapResult = new CakewalkModelMapper().Map( source );

        Assert.That( mapResult.IsSuccess, Is.True, "Mapping should succeed" );

        var dest = Path.GetTempFileName();


        try
        {
            await using( var fileWriter = new LocalTextContentWriter( dest ) )
            {
                var exporter = new CakewalkExporter();

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

public class CakewalkModelMapper : IModelMapper<CakewalkArticulationMap>
{
    public Result<CakewalkArticulationMap, ExportReason> Map( UniversalDefinitionProductSet source )
    {
        throw new NotImplementedException();
    }
}

public class CakewalkExporter : IDefinitionExporter
{
    public async Task<Result<Unit, ExportReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinitionProductSet source,
        CancellationToken cancellationToken = default )
    {
        throw new NotImplementedException();
    }
}
