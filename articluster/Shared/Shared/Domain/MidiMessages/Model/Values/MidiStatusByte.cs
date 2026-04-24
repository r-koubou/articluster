using ArtiCluster.Commons.ValueObjects;

namespace ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

public sealed record MidiStatusByte : IntValueObject
{
    public static readonly MidiStatusByte Null = new();

    private MidiStatusByte() : base( -1 ) {}

    public MidiStatusByte( int value ) : base( value, minValue: 0x80, maxValue: 0xFF ) {}
}
