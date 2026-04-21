namespace ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

public sealed record MidiNoteNumber : MidiMessageByte
{
    public static readonly MidiNoteNumber Null = new();

    private MidiNoteNumber() : base( -1 ) {}
    public MidiNoteNumber( int Value ) : base( Value, minValue: 0, maxValue: 127 ) {}
}
