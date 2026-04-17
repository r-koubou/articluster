using ArtiCluster.Shared.ValueObjects;

namespace ArtiCluster.Shared.Domain.Articulation.Model.Values.Midi;

public record MidiMessageByte : IntValueObject
{
    private const int MinValue = 0;
    private const int MaxValue = 127;

    protected MidiMessageByte( int value ) : base( value )
    {
        ValueOutOfRangeException.ThrowIf( this, MinValue, MaxValue );
    }
}
