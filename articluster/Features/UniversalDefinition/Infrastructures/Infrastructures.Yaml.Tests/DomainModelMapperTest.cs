using System;
using System.Collections.Generic;
using System.Linq;

using ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model;
using ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model.MidiMessages;
using ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

using NUnit.Framework;

namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Tests;

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
                    NoteOn =
                    [
                        new MidiNoteOnMessageModel( channel: 1, noteNumber: 60, velocity: 110 )
                    ],
                    NoteOff =
                    [
                        new MidiNoteOffMessageModel( channel: 2, noteNumber: 61, velocity: 111 )
                    ],
                    ControlChange =
                    [
                        new MidiControlChangeMessageModel( channel: 3, controlNumber: 64, controlValue: 112 )
                    ],
                    ProgramChange =
                    [
                        new MidiProgramChangeMessageModel( channel: 4, programNumber: 42 )
                    ],
                    Extra = new Dictionary<string, string>
                    {
                        { "LocalKey", "LocalValue" }
                    }
                }
            ]
        };

        var mapper = new DomainModelMapper();
        var actual = mapper.Map( source );

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
                Assert.That( assignment.MidiNoteOn.Single().Channel.Value, Is.EqualTo( 1 ) );
                Assert.That( assignment.MidiNoteOn.Single().DataByte1.Value, Is.EqualTo( 60 ) );
                Assert.That( assignment.MidiNoteOn.Single().DataByte2.Value, Is.EqualTo( 110 ) );
                Assert.That( assignment.MidiNoteOff.Single().Channel.Value, Is.EqualTo( 2 ) );
                Assert.That( assignment.MidiNoteOff.Single().DataByte1.Value, Is.EqualTo( 61 ) );
                Assert.That( assignment.MidiNoteOff.Single().DataByte2.Value, Is.EqualTo( 111 ) );
                Assert.That( assignment.MidiControlChange.Single().Channel.Value, Is.EqualTo( 3 ) );
                Assert.That( assignment.MidiControlChange.Single().DataByte1.Value, Is.EqualTo( 64 ) );
                Assert.That( assignment.MidiControlChange.Single().DataByte2.Value, Is.EqualTo( 112 ) );
                Assert.That( assignment.MidiProgramChange.Single().Channel.Value, Is.EqualTo( 4 ) );
                Assert.That( assignment.MidiProgramChange.Single().DataByte1.Value, Is.EqualTo( 42 ) );
                Assert.That( assignment.MidiProgramChange.Single().DataByte2, Is.EqualTo( NullMidiDataByte.Instance ) );
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

        var mapper = new DomainModelMapper();

        var actual = mapper.Map( source );

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
