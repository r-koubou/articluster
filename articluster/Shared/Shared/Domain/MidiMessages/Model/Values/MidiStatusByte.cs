namespace ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

public sealed record MidiStatusByte : MidiMessageByte
{
    public static readonly MidiStatusByte Null = new();

    public MidiChannel Channel { get; init; }

    private MidiStatusByte() : base( -1 )
    {
        Channel = MidiChannel.Null;
    }

    public MidiStatusByte( int value ) : base( value, minValue: 0x00, maxValue: 0xFF )
    {
        Channel = new MidiChannel( value & 0x0F );
    }
}
