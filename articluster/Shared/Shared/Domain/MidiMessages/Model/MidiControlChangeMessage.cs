using ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

namespace ArtiCluster.Shared.Domain.MidiMessages.Model;

public sealed record MidiControlChangeMessage : IMidiChannelVoiceMessage
{
    public static readonly MidiControlChangeMessage Null = new();

    public MidiChannel Channel { get; init; }
    public MidiMessageByte StatusByte { get; init; }
    public MidiMessageByte DataByte1 { get; init; }
    public MidiMessageByte DataByte2 { get; init; }

    private MidiControlChangeMessage()
    {
        Channel    = MidiChannel.Null;
        StatusByte = MidiStatusByte.Null;
        DataByte1  = MidiControlChangeValue.Null;
        DataByte2  = MidiControlChangeValue.Null;
    }

    public MidiControlChangeMessage( int channel, int controlNumber, int controlValue )
    {
        Channel    = new MidiChannel( channel );
        StatusByte = new MidiStatusByte( 0xB0 | channel );
        DataByte1  = new MidiControlChangeValue( controlNumber );
        DataByte2  = new MidiControlChangeValue( controlValue );
    }
}
