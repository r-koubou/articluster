namespace ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

public sealed record MidiProgramNumber : MidiMessageByte
{
    public static readonly MidiProgramNumber Null = new();

    private MidiProgramNumber() : base( -1 ) {}
    public MidiProgramNumber( int Value ) : base( Value, minValue: 0, maxValue: 127 ) {}
}
