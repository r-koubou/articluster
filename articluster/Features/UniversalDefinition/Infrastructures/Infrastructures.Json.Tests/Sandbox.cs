using System;
using System.Collections.Generic;
using System.Text.Json;

using ArtiCluster.Features.UniversalDefinition.Infrastructures.Json.Model;

using NUnit.Framework;

namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Json.Tests;

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

        var option = new JsonSerializerOptions
        {
            WriteIndented = true,
            IndentSize = 2
        };

        var jsonText = JsonSerializer.Serialize( model, option );
        Console.WriteLine( jsonText );

        Assert.Pass();
    }
}

[Flags]
public enum MidiMessageType
{
    None = 0x00,

    // Channel voice message
    NoteOff = 0x80,
    NoteOn = 0x90,
    PolyphonicKeyPressure = 0xA0,
    ControlChange = 0xB0,
    ProgramChange = 0xC0,
    ChannelPressure = 0xD0,
    PitchBendChange = 0xE0,

    // Channel mode message
    AllSoundOff = 0xB0,
    ResetAllController = 0xB0,
    LocalControl = 0xB0,
    AllNotesOff = 0xB0,
    OmniOff = 0xB0,
    OmniOn = 0xB0,
    MonoMode = 0xB0,
    PolyMode = 0xB0,

    // System common message
    SysExBegin = 0xF0,
    SysExMidiTimeRecord = 0xF1,
    SysExSongPosition = 0xF2,
    SysExSongSelect = 0xF3,
    SysExChainRequest = 0xF6,
    SysExEnd = 0xF7,

    // System realtime message
    MidiClock = 0xF8,
    Start = 0xFA,
    Continue = 0xFB,
    Stop = 0xFC,
    ActiveSensing = 0xFE,
    Reset = 0xFF
}
