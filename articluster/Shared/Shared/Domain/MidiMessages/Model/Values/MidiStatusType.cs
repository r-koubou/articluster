namespace ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

public enum MidiStatusType
{
    Undefined = -1,

    #region Channel Voice Message
    NoteOff = 0x80,
    NoteOn = 0x90,
    PolyphonicKeyPressure = 0xA0,
    ControlChangeOrChannelVoiceMessage = 0xB0,
    ProgramChange = 0xC0,
    ChannelPressure = 0xD0,
    PitchBendChange = 0xE0,
    #endregion

    #region System Common Message
    SysExBegin = 0xF0,
    MidiTimeRecord = 0xF1,
    SongPosition = 0xF2,
    SongSelect = 0xF3,
    ChainRequest = 0xF6,
    SysExEnd = 0xF7,
    #endregion

    #region System Realtime Message
    MidiClock = 0xF8,
    Start = 0xFA,
    Continue = 0xFB,
    Stop = 0xFC,
    ActiveSensing = 0xFE,
    Reset = 0xFF
    #endregion
}
