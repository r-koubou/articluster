using System;
using System.Collections.Generic;

using ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model;

using NUnit.Framework;

using YamlDotNet.Serialization;

namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Tests;

[TestFixture]
public class Sandbox
{
    [Test]
    public void Test()
    {
        var model = new ArticulationModel
        {
            Id               = Guid.NewGuid(),
            Author           = "John Doe",
            ManufacturerName = "Acme Corp",
            ProductName      = "Super Synth",
            PatchName        = "Epic Lead",
            Description      = "This is a multi-line description of the articulation."
        };

        model.Assignments.Add( new AssignmentModel
            {
                MidiMessage = new MidiMessageModel
                {
                    Status = 0x90,
                    Data1  = 60,
                    Data2  = 127
                },
                Extra = new Dictionary<string, string>
                {
                    { "Key1", "LocalValue1" },
                    { "Key2", "LocalValue2" }
                }
            }
        );
        model.Assignments.Add( new AssignmentModel
            {
                MidiMessage = new MidiMessageModel
                {
                    Status = 0x80,
                    Data1  = 60,
                    Data2  = 127
                }
            }
        );

        model.Extra = new Dictionary<string, string>
        {
            { "GlobalKey1", "GlobalValue1" },
            { "GlobalKey2", "GlobalValue2" }
        };

        var builder = new SerializerBuilder().Build();

        builder.Serialize( Console.Out, model );

        Assert.Pass();
    }
}
