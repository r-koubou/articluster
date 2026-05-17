using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using ArtiCluster.Commons.IO;
using ArtiCluster.Commons.Text;
using ArtiCluster.Features.Cubase15.ExpressionMaps.Exports;
using ArtiCluster.Features.Cubase15.ExpressionMaps.Mappers;
using ArtiCluster.Features.Cubase15.ExpressionMaps.Models;
using ArtiCluster.Features.Cubase15.ExpressionMaps.Models.XMLElements;
using ArtiCluster.Shared.IO.Local;
using ArtiCluster.Shared.Mock;

using NUnit.Framework;

namespace ArtiCluster.Features.Cubase15.ExpressionMaps.Tests;

/// <summary>
/// Unit Test for Cubase 15 and later
/// </summary>
[TestFixture]
public class CubaseSerializationTest
{
    [Test]
    public void SerializeTest()
    {
        var id = Guid.NewGuid();
        var source = MockUniversalDefinition.CreateDefinition( id, patchName: "Epic Lead" );

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
        var source = MockUniversalDefinition.CreateDefinition( id, patchName: "Epic Lead" );

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

    [Test]
    public async Task ExportMultipleArticulationGroupTest()
    {
        var id = Guid.NewGuid();
        var source = MockUniversalDefinition.CreateMultiArticulationGroupDefinition( id, patchName: "Epic Lead" );

        var mapResult = new CubaseModelMapper().Map( source );

        Assert.That( mapResult.IsSuccess, Is.True, "Mapping should succeed" );

        var xmlModel = mapResult.Unwrap();
        var hasTechniqueGroups = xmlModel.Member.Exists( m => m.Name == "techniqueGroups" );

        Assert.That( hasTechniqueGroups, Is.True, "Model should contain techniqueGroups" );

        var techniqueGroups = xmlModel.Member.Single( m => m.Name == "techniqueGroups" );
        Assert.That( techniqueGroups.List.Count, Is.EqualTo( 1 ) );
        Assert.That( techniqueGroups.List.First().Obj.Count, Is.EqualTo( source.ArticulationGroups.Count ) );

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
