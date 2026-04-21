using ArtiCluster.Commons.ValueObjects;

namespace ArtiCluster.Shared.Domain.Articulation.Model.Values.Midi;

public sealed record MidiMessageByte : IntValueObject
{
    // ReSharper disable MemberCanBePrivate.Global
    public const int MinValue = 0;
    public const int MaxValue = 127;
    // ReSharper restore MemberCanBePrivate.Global

    public static readonly MidiMessageByte Null = new();

    private MidiMessageByte() : base( -1 ) {}

    public MidiMessageByte( int Value ) : base( Value )
    {
        ValueOutOfRangeException.ThrowIf( Value, MinValue, MaxValue );
    }
}
