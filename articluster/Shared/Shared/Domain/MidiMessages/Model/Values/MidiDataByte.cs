using ArtiCluster.Commons.ValueObjects;

namespace ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

public sealed record MidiDataByte : IntValueObject
{
    public static readonly MidiDataByte None = new();

    private MidiDataByte() : base( -1 ) {}

    public MidiDataByte( int value ) : base( value, 0x00, 0x7F ) {}

}
