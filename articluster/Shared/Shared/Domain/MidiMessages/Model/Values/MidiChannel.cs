namespace ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

public sealed record MidiChannel : MidiMessageByte
{
    public static readonly MidiChannel Null = new();

    private MidiChannel() : base( -1 ) {}
    public MidiChannel( int value ) : base( value, minValue: 0, maxValue: 15 ) {}
}
