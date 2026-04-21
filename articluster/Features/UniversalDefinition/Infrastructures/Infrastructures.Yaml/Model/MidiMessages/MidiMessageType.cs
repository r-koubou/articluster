using System;

namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model.MidiMessages;

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
