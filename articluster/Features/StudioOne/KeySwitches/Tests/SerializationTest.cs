using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using ArtiCluster.Commons.IO;
using ArtiCluster.Commons.Text;
using ArtiCluster.Features.StudioOne.KeySwitches.Exports;
using ArtiCluster.Features.StudioOne.KeySwitches.Mappers;
using ArtiCluster.Features.StudioOne.KeySwitches.Models;
using ArtiCluster.Shared.IO.Local;
using ArtiCluster.Shared.Mock;

using NUnit.Framework;

namespace ArtiCluster.Features.StudioOne.KeySwitches.Tests;

[TestFixture]
public class SerializationTest
{
    [Test]
    public void SerializeTest()
    {
        var id = Guid.NewGuid();
        var source = MockUniversalDefinition.CreateDefinition( id, patchName: "Epic Lead" );

        var mapResult = new StudioOneModelMapper().Map( source );

        Assert.That( mapResult.IsSuccess, Is.True, "Mapping should succeed" );

        var serializer = new XmlSerializer( typeof( StudioOneRootElement ) );
        // no xmlns adding
        // see: https://stackoverflow.com/a/8882612
        var xmlNamespaces = new XmlSerializerNamespaces();
        xmlNamespaces.Add( "", "" );

        var stringWriter = new StringWriterWithEncoding( EncodingConstants.Utf8NoBom );
        var xmlWriterSettings = new XmlWriterSettings
        {
            Indent = true,
            Async = true
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

        var dest = Path.GetTempFileName();

        try
        {
            await using( var fileWriter = new LocalTextContentWriter( dest ) )
            {
                var exporter = new StudioOneExporter();

                var result = await exporter.ExportAsync( fileWriter, source );
                Assert.That( result.IsSuccess, Is.True, "Export should succeed" );
            }

            await TestContext.Out.WriteAsync( await File.ReadAllTextAsync( dest ) );
        }
        finally
        {
            File.Delete( dest );
        }
    }

    [Test]
    public async Task ExportMultiArticulationGroupTest()
    {
        var id = Guid.NewGuid();
        var source = MockUniversalDefinition.CreateMultiArticulationGroupDefinition( id, patchName: "Epic Lead" );

        var dest = Path.GetTempFileName();

        try
        {
            await using( var fileWriter = new LocalTextContentWriter( dest ) )
            {
                var exporter = new StudioOneExporter();

                var result = await exporter.ExportAsync( fileWriter, source );
                Assert.That( result.IsSuccess, Is.True, "Export should succeed" );
            }

            await TestContext.Out.WriteAsync( await File.ReadAllTextAsync( dest ) );
        }
        finally
        {
            File.Delete( dest );
        }
    }
}
