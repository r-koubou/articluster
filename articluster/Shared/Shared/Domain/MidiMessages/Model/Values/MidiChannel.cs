using ArtiCluster.Commons.ValueObjects;

namespace ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

public sealed record MidiChannel : IntValueObject
{
    public static readonly MidiChannel None = new();

    private MidiChannel() : base( -1 ) {}
    public MidiChannel( int value ) : base( value, minValue: 0, maxValue: 15 ) {}
}
