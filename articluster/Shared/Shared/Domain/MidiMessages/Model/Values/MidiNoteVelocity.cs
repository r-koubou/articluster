namespace ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

public sealed record MidiNoteVelocity : MidiMessageByte
{
    public static readonly MidiNoteVelocity Null = new();

    private MidiNoteVelocity() : base( -1 ) {}
    public MidiNoteVelocity( int Value ) : base( Value, minValue: 0, maxValue: 127 ) {}
}
