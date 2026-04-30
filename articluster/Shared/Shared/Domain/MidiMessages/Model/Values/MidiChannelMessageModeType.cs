namespace ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

public enum MidiChannelMessageModeType
{
    Undefined = -1,

    AllSoundOff = 0x78,
    ResetAllController = 0x79,
    LocalControl = 0x7A,
    NotesOff = 0x7B,
    OmniOff = 0x7C,
    OmniOn = 0x7D,
    MonoMode = 0x7E,
    PolyMode = 0x7F,
}
