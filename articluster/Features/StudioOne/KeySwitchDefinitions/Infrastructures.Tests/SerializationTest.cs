using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using ArtiCluster.Commons.IO;
using ArtiCluster.Features.StudioOne.KeySwitchDefinitions.Infrastructures.Model;
using ArtiCluster.Shared.Domain.MidiMessages.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

using NUnit.Framework;

namespace ArtiCluster.Features.StudioOne.KeySwitchDefinitions.Infrastructures.Tests;

[TestFixture]
public class SerializationTest
{
    [Test]
    public void SerializeTest()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();

        var source1 = CreateMock( id1, patchName: "Epic Lead" );
        var source2 = CreateMock( id2, patchName: "E.Bass" );

        var productSet = new UniversalDefinitionProductSet(
            manufacturerName: source1.ManufacturerName,
            productName: source1.ProductName,
            items: [ source1, source2 ]
        );

        var mapResult = new StudioOneModelMapper().Map( productSet );

        Assert.That( mapResult.IsSuccess, Is.True, "Mapping should succeed" );

        var serializer = new XmlSerializer( typeof( StudioOneRootElement ) );
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

    private static UniversalDefinition CreateMock(
        Guid id,
        string manufacturerName = "Acme Corp",
        string productName = "Super Synth",
        string patchName = "Epic Lead" )
    {
        return UniversalDefinition.Create(
            id: id,
            author: "John Doe",
            manufacturerName: manufacturerName,
            productName: productName,
            patchName: patchName,
            description: "multi-line\ndescription",
            extra: new Dictionary<string, string>
            {
                { "GlobalKey1", "GlobalValue1" },
                { "GlobalKey2", "GlobalValue2" }
            },
            articulations:
            [
                Articulation.Create(
                    name: "Sustain",
                    midiMessages:
                    [
                        // Note On
                        MidiMessage.Create( 0x90, 40, 100 ),
                        // Note Off
                        MidiMessage.Create( 0x80, 40, 110 ),
                        // Control Change
                        MidiMessage.Create( 0xB0, 1, 127 ),
                        // Program Change
                        MidiMessage.Create( 0xC0, 49 ),
                    ],
                    extra: new Dictionary<string, string>
                    {
                        { "LocalKey", "LocalValue" }
                    }
                )
            ]
        );
    }
}
