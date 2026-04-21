using System;
using System.Collections.Generic;

using ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model;
using ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model.MidiMessages;

using NUnit.Framework;

using YamlDotNet.Serialization;

namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Tests;

[TestFixture]
public class Sandbox
{
    [Test]
    public void Test()
    {
        var model = new RootModel
        {
            Id               = Guid.NewGuid(),
            Author           = "John Doe",
            ManufacturerName = "Acme Corp",
            ProductName      = "Super Synth",
            PatchName        = "Epic Lead",
            Description      = """
                               This is a multi-line
                               description of the articulation.
                               """
        };

        model.Assignments.Add( new AssignmentModel
            {
                Name = "Sustain",
                NoteOff =
                [
                    new MidiNoteOffMessageModel( channel: 0, noteNumber: 60, velocity: 127 ),
                ],
                Extra = new Dictionary<string, string>
                {
                    { "Key1", "LocalValue1" },
                    { "Key2", "LocalValue2" }
                }
            }
        );
        model.Assignments.Add( new AssignmentModel
            {
                Name = "Staccato",
                NoteOn =
                [
                    new MidiNoteOnMessageModel( channel: 0, noteNumber: 60, velocity: 127 ),
                ],
                ControlChange = [
                    new MidiControlChangeMessageModel( channel: 0, controlNumber: 64, controlValue: 127 ),
                ],
                ProgramChange = [
                    new MidiProgramChangeMessageModel( channel: 0, programNumber: 41 ),
                ]
            }
        );

        model.Extra = new Dictionary<string, string>
        {
            { "GlobalKey1", "GlobalValue1" },
            { "GlobalKey2", "GlobalValue2" }
        };

        var builder =
            new SerializerBuilder()
               .ConfigureDefaultValuesHandling( DefaultValuesHandling.OmitEmptyCollections )
               .Build();

        builder.Serialize( Console.Out, model );

        Assert.Pass();
    }
}
