namespace ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

public sealed record MidiControlChangeValue : MidiMessageByte
{
    public static readonly MidiControlChangeValue Null = new();

    private MidiControlChangeValue() : base( -1 ) {}
    public MidiControlChangeValue( int Value ) : base( Value, minValue: 0, maxValue: 127 ) {}
}
