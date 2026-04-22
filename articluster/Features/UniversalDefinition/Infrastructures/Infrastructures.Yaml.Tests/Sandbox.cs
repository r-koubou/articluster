using System;
using System.Collections.Generic;

using ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model;

using NUnit.Framework;

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
            Description = """
                          This is a multi-line
                          description of the articulation.
                          """
        };

        model.Assignments.Add( new AssignmentModel
            {
                Name = "Sustain",
                MidiMessages =
                [
                    // Note On
                    new MidiMessageModel( 0x90, 40, 100 ),
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
                MidiMessages =
                [
                    // Note On
                    new MidiMessageModel( 0x90, 40, 100 ),
                    // Control Change
                    new MidiMessageModel( 0xB0, 1, 127 ),
                    // Program Change
                    new MidiMessageModel( 0xC0, 49 ),
                ],
            }
        );

        model.Extra = new Dictionary<string, string>
        {
            { "GlobalKey1", "GlobalValue1" },
            { "GlobalKey2", "GlobalValue2" }
        };

        var serializer = SerializationConstants.DefaultSerializer;

        serializer.Serialize( Console.Out, model );

        Assert.Pass();
    }
}
