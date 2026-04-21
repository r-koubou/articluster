namespace ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

public sealed record MidiProgramNumber : MidiMessageByte
{
    public static readonly MidiProgramNumber Null = new();

    private MidiProgramNumber() : base( -1 ) {}
    public MidiProgramNumber( int value ) : base( value, minValue: 0, maxValue: 127 ) {}
}
