using ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

namespace ArtiCluster.Shared.Domain.MidiMessages.Model;

public sealed record MidiProgramChangeMessage : IMidiChannelVoiceMessage
{
    public static readonly MidiProgramChangeMessage Null = new();

    public MidiChannel Channel { get; init; }
    public MidiMessageByte StatusByte { get; init; }
    public MidiMessageByte DataByte1 { get; init; }
    public MidiMessageByte DataByte2 { get; init; }

    private MidiProgramChangeMessage()
    {
        Channel    = MidiChannel.Null;
        StatusByte = MidiStatusByte.Null;
        DataByte1  = MidiProgramNumber.Null;
        DataByte2  = NullMidiDataByte.Instance;
    }

    public MidiProgramChangeMessage( int channel, int programNumber )
    {
        Channel    = new MidiChannel( channel );
        StatusByte = new MidiStatusByte( 0xC0 | channel );
        DataByte1  = new MidiProgramNumber( programNumber );
        DataByte2  = NullMidiDataByte.Instance;
    }
}
