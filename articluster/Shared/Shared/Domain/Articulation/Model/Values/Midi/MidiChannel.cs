using ArtiCluster.Commons.ValueObjects;

namespace ArtiCluster.Shared.Domain.Articulation.Model.Values.Midi;

public sealed record MidiChannel : IntValueObject
{
    // ReSharper disable MemberCanBePrivate.Global
    public const int MinValue = 0;
    public const int MaxValue = 15;
    // ReSharper restore MemberCanBePrivate.Global

    public static readonly MidiChannel Null = new();

    private MidiChannel() : base( -1 ) {}

    public MidiChannel( int Value ) : base( Value )
    {
        ValueOutOfRangeException.ThrowIf( this, MinValue, MaxValue );
    }
}
