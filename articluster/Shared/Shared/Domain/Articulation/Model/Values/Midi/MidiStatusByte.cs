using ArtiCluster.Commons.ValueObjects;

namespace ArtiCluster.Shared.Domain.Articulation.Model.Values.Midi;

public sealed record MidiStatusByte : IntValueObject
{
    // ReSharper disable MemberCanBePrivate.Global
    public const int MinValue = 0x80;
    public const int MaxValue = 0xFF;
    // ReSharper restore MemberCanBePrivate.Global

    public static readonly MidiStatusByte Null = new();

    public MidiChannel Channel
        => new( Value & 0x0F );

    private MidiStatusByte() : base( -1 ) {}

    public MidiStatusByte( int value ) : base( value )
    {
        ValueOutOfRangeException.ThrowIf( Value, MinValue, MaxValue );
    }

    public MidiStatusByte( int value, MidiChannel channel ) : base( value | channel.Value )
    {
        ValueOutOfRangeException.ThrowIf( Value, MinValue, MaxValue );
    }
}
