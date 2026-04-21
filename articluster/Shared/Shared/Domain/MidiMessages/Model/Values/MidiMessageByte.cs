using ArtiCluster.Commons.ValueObjects;

namespace ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

public abstract record MidiMessageByte : IntValueObject
{
    protected MidiMessageByte( int value ) : base( value ) {}
    protected MidiMessageByte( int value, int? minValue, int? maxValue ) : base( value, minValue, maxValue ) {}
}

public sealed record NullMidiDataByte : MidiMessageByte
{
    public static readonly NullMidiDataByte Instance = new();

    private NullMidiDataByte() : base( -1 ) {}
}
