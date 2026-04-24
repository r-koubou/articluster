using System;
using System.Collections.Generic;
using System.Linq;

using ArtiCluster.Features.UniversalDefinitions.Infrastructures.Yaml.Model;
using ArtiCluster.Shared.Domain.MidiMessages.Model;
using ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

using NUnit.Framework;

namespace ArtiCluster.Features.UniversalDefinitions.Infrastructures.Yaml.Tests;

[TestFixture]
public class DomainModelMapperTest
{
    [Test]
    public void MapsRootModelToArticulationTest()
    {
        var id = Guid.NewGuid();
        var source = new RootModel
        {
            Id               = id,
            Author           = "John Doe",
            ManufacturerName = "Acme Corp",
            ProductName      = "Super Synth",
            PatchName        = "Epic Lead",
            Description      = "multi-line\ndescription",
            Extra = new Dictionary<string, string>
            {
                { "GlobalKey1", "GlobalValue1" },
                { "GlobalKey2", "GlobalValue2" }
            },
            Assignments =
            [
                new AssignmentModel
                {
                    Name = "Sustain",
                    MidiMessages =
                    [
                        // Note On
                        new MidiMessageModel( 0x90, 40, 100 ),
                        // Note Off
                        new MidiMessageModel( 0x80, 40, 110 ),
                        // Control Change
                        new MidiMessageModel( 0xB0, 1, 127 ),
                        // Program Change
                        new MidiMessageModel( 0xC0, 49  ),
                    ],
                    Extra = new Dictionary<string, string>
                    {
                        { "LocalKey", "LocalValue" }
                    }
                }
            ]
        };

        var actual = DomainModelMapper.Map( source );

        Assert.Multiple( () =>
            {
                Assert.That( actual.Id, Is.EqualTo( id ) );
                Assert.That( actual.Author.Value, Is.EqualTo( source.Author ) );
                Assert.That( actual.ManufacturerName.Value, Is.EqualTo( source.ManufacturerName ) );
                Assert.That( actual.ProductName.Value, Is.EqualTo( source.ProductName ) );
                Assert.That( actual.PatchName.Value, Is.EqualTo( source.PatchName ) );
                Assert.That( actual.Description.Value, Is.EqualTo( source.Description ) );
                Assert.That( actual.Extra, Is.EqualTo( source.Extra ) );
                Assert.That( actual.Assignments, Has.Count.EqualTo( 1 ) );
            }
        );

        var assignment = actual.Assignments.Single();

        Assert.Multiple( () =>
            {
                Assert.That( assignment.Name.Value, Is.EqualTo( "Sustain" ) );
                Assert.That( assignment.Extra, Is.EqualTo( source.Assignments[ 0 ].Extra ) );

                var midiMessages = new List<MidiMessage>( actual.Assignments.Single().MidiMessages );

                Assert.That( midiMessages.Count, Is.EqualTo( 4 ) );

                // Note On
                Assert.That( midiMessages[ 0 ].Status.Value, Is.EqualTo( 0x90 ) );
                Assert.That( midiMessages[ 0 ].Data1.Value, Is.EqualTo( 40 ) );
                Assert.That( midiMessages[ 0 ].Data2.Value, Is.EqualTo( 100 ) );
                // Note Off
                Assert.That( midiMessages[ 1 ].Status.Value, Is.EqualTo( 0x80 ) );
                Assert.That( midiMessages[ 1 ].Data1.Value, Is.EqualTo( 40 ) );
                Assert.That( midiMessages[ 1 ].Data2.Value, Is.EqualTo( 110 ) );
                // Control Change
                Assert.That( midiMessages[ 2 ].Status.Value, Is.EqualTo( 0xB0 ) );
                Assert.That( midiMessages[ 2 ].Data1.Value, Is.EqualTo( 1 ) );
                Assert.That( midiMessages[ 2 ].Data2.Value, Is.EqualTo( 127 ) );
                // Program Change
                Assert.That( midiMessages[ 3 ].Status.Value, Is.EqualTo( 0xC0 ) );
                Assert.That( midiMessages[ 3 ].Data1.Value, Is.EqualTo( 49 ) );
                Assert.That( midiMessages[ 3 ].Data2.Value, Is.EqualTo( MidiDataByte.None.Value ) );
            }
        );
    }

    [Test]
    public void ClonesMutableCollectionsTest()
    {
        var source = new RootModel
        {
            Author           = "John Doe",
            ManufacturerName = "Acme Corp",
            ProductName      = "Super Synth",
            PatchName        = "Epic Lead",
            Assignments =
            [
                new AssignmentModel
                {
                    Name = "Sustain",
                    Extra = new Dictionary<string, string>
                    {
                        { "LocalKey", "LocalValue" }
                    }
                }
            ],
            Extra = new Dictionary<string, string>
            {
                { "GlobalKey", "GlobalValue" }
            }
        };

        var actual = DomainModelMapper.Map( source );

        source.Extra[ "GlobalKey" ]                 = "Updated";
        source.Assignments[ 0 ].Extra[ "LocalKey" ] = "Updated";

        Assert.Multiple( () =>
            {
                Assert.That( actual.Extra[ "GlobalKey" ], Is.EqualTo( "GlobalValue" ) );
                Assert.That( actual.Assignments.Single().Extra[ "LocalKey" ], Is.EqualTo( "LocalValue" ) );
            }
        );
    }
}
