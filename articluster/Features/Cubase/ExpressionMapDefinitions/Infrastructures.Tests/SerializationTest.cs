using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using ArtiCluster.Commons.IO;
using ArtiCluster.Features.Cubase.ExpressionMapDefinitions.Infrastructures.Model;
using ArtiCluster.Shared.IO.Local;
using ArtiCluster.Shared.Mock;

using NUnit.Framework;

namespace ArtiCluster.Features.Cubase.ExpressionMapDefinitions.Infrastructures.Tests;

[TestFixture]
public class SerializationTest
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

        var stringWriter = new StringWriterWithEncoding( Encoding.UTF8 );
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
}
