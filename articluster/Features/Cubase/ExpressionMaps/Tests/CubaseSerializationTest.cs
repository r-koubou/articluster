using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using ArtiCluster.Commons.IO;
using ArtiCluster.Commons.Text;
using ArtiCluster.Features.Cubase.ExpressionMaps.Exports;
using ArtiCluster.Features.Cubase.ExpressionMaps.Mappers;
using ArtiCluster.Features.Cubase.ExpressionMaps.Models;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Local;
using ArtiCluster.Shared.Mock;

using NUnit.Framework;

namespace ArtiCluster.Features.Cubase.ExpressionMaps.Tests;

[TestFixture]
public class CubaseSerializationTest
{
    [Test]
    public void SerializeTest()
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

        var mapResult = new CubaseModelMapper().Map( source );

        Assert.That( mapResult.IsSuccess, Is.True, "Mapping should succeed" );

        var serializer = new XmlSerializer( typeof( RootElement ) );
        // no xmlns adding
        // see: https://stackoverflow.com/a/8882612
        var xmlNamespaces = new XmlSerializerNamespaces();
        xmlNamespaces.Add( "", "" );

        var stringWriter = new StringWriterWithEncoding( EncodingConstants.Utf8NoBom );
        var xmlWriterSettings = new XmlWriterSettings
        {
            Indent = true
        };

        using var xmlWriter = XmlWriter.Create( stringWriter, xmlWriterSettings );
        serializer.Serialize( xmlWriter, mapResult.Unwrap(), xmlNamespaces );

        Assert.That( mapResult.IsSuccess, Is.True, "Mapping should succeed" );

        TestContext.Out.WriteLine( stringWriter.ToString() );
    }

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

        var mapResult = new CubaseModelMapper().Map( source );

        Assert.That( mapResult.IsSuccess, Is.True, "Mapping should succeed" );

        var dest = Path.GetTempFileName();


        try
        {
            await using( var fileWriter = new LocalTextContentWriter( dest ) )
            {
                var exporter = new CubaseExporter();

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
