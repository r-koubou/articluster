namespace ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

public sealed record MidiControlChangeNumber : MidiMessageByte
{
    public static readonly MidiControlChangeNumber Null = new();

    private MidiControlChangeNumber() : base( -1 ) {}
    public MidiControlChangeNumber( int value ) : base( value, minValue: 0, maxValue: 127 ) {}
}
