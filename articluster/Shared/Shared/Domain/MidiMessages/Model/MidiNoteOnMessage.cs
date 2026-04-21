using ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

namespace ArtiCluster.Shared.Domain.MidiMessages.Model;

public sealed record MidiNoteOnMessage : IMidiChannelVoiceMessage
{
    public static readonly MidiNoteOnMessage Null = new();

    public MidiChannel Channel { get; init; }
    public MidiMessageByte StatusByte { get; init; }
    public MidiMessageByte DataByte1 { get; init; }
    public MidiMessageByte DataByte2 { get; init; }

    private MidiNoteOnMessage()
    {
        Channel    = MidiChannel.Null;
        StatusByte = MidiStatusByte.Null;
        DataByte1  = MidiNoteNumber.Null;
        DataByte2  = MidiNoteVelocity.Null;
    }

    public MidiNoteOnMessage( int channel, int noteNumber, int velocity )
    {
        Channel    = new MidiChannel( channel );
        StatusByte = new MidiStatusByte( 0x90 | channel );
        DataByte1  = new MidiNoteNumber( noteNumber );
        DataByte2  = new MidiNoteVelocity( velocity );
    }
}
