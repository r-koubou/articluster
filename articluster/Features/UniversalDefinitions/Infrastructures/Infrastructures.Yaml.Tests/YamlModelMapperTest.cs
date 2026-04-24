using System;
using System.Collections.Generic;
using System.Linq;

using ArtiCluster.Features.UniversalDefinitions.Infrastructures.Yaml.Model;
using ArtiCluster.Shared.Domain.Articulation.Model;
using ArtiCluster.Shared.Domain.MidiMessages.Model;

using NUnit.Framework;

namespace ArtiCluster.Features.UniversalDefinitions.Infrastructures.Yaml.Tests;

[TestFixture]
public class YamlModelMapperTest
{
    [Test]
    public void MapsRootModelToArticulationTest()
    {
        var id = Guid.NewGuid();
        var source = new UniversalDefinition(
            id: id,
            author: "John Doe",
            manufacturerName: "Acme Corp",
            productName: "Super Synth",
            patchName: "Epic Lead",
            description: "multi-line\ndescription",
            extra: new Dictionary<string, string>
            {
                { "GlobalKey1", "GlobalValue1" },
                { "GlobalKey2", "GlobalValue2" }
            },
            articulations:
            [
                new Articulation(
                    name: "Sustain",
                    midiMessages:
                    [
                        // Note On
                        new MidiMessage( 0x90, 40, 100 ),
                        // Note Off
                        new MidiMessage( 0x80, 40, 110 ),
                        // Control Change
                        new MidiMessage( 0xB0, 1, 127 ),
                        // Program Change
                        new MidiMessage( 0xC0, 49 ),
                    ],
                    extra: new Dictionary<string, string>
                    {
                        { "LocalKey", "LocalValue" }
                    }
                )
            ]
        );

        var actual = YamlModelMapper.Map( source );

        Assert.Multiple( () =>
            {
                Assert.That( actual.Id, Is.EqualTo( id ) );
                Assert.That( actual.Author, Is.EqualTo( source.Author.Value ) );
                Assert.That( actual.ManufacturerName, Is.EqualTo( source.ManufacturerName.Value ) );
                Assert.That( actual.ProductName, Is.EqualTo( source.ProductName.Value ) );
                Assert.That( actual.PatchName, Is.EqualTo( source.PatchName.Value ) );
                Assert.That( actual.Description, Is.EqualTo( source.Description.Value ) );
                Assert.That( actual.Extra, Is.EqualTo( source.Extra ) );
                Assert.That( actual.Articulations, Has.Count.EqualTo( 1 ) );
            }
        );

        var assignment = actual.Articulations.Single();

        Assert.Multiple( () =>
            {
                Assert.That( assignment.Name, Is.EqualTo( "Sustain" ) );
                Assert.That( assignment.Extra, Is.EqualTo( source.Articulations.Single().Extra ) );

                var midiMessages = new List<MidiMessageModel>( actual.Articulations.Single().MidiMessages );

                Assert.That( midiMessages.Count, Is.EqualTo( 4 ) );

                // Note On
                Assert.That( midiMessages[ 0 ].Status, Is.EqualTo( 0x90 ) );
                Assert.That( midiMessages[ 0 ].Data1, Is.EqualTo( 40 ) );
                Assert.That( midiMessages[ 0 ].Data2, Is.EqualTo( 100 ) );
                // Note Off
                Assert.That( midiMessages[ 1 ].Status, Is.EqualTo( 0x80 ) );
                Assert.That( midiMessages[ 1 ].Data1, Is.EqualTo( 40 ) );
                Assert.That( midiMessages[ 1 ].Data2, Is.EqualTo( 110 ) );
                // Control Change
                Assert.That( midiMessages[ 2 ].Status, Is.EqualTo( 0xB0 ) );
                Assert.That( midiMessages[ 2 ].Data1, Is.EqualTo( 1 ) );
                Assert.That( midiMessages[ 2 ].Data2, Is.EqualTo( 127 ) );
                // Program Change
                Assert.That( midiMessages[ 3 ].Status, Is.EqualTo( 0xC0 ) );
                Assert.That( midiMessages[ 3 ].Data1, Is.EqualTo( 49 ) );
                Assert.That( midiMessages[ 3 ].Data2, Is.Null );
            }
        );
    }
}
