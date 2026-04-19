using ArtiCluster.Commons.ValueObjects;

namespace ArtiCluster.Shared.IO.Stream.ValueObjects;

public sealed record ReadLength : IntValueObject
{
    public static readonly ReadLength ToEnd = new();

    private ReadLength() : base( -1 ) {}

    public ReadLength( int value ) : base( value )
    {
        ValueOutOfRangeException.ThrowIf( this, 0, int.MaxValue );
    }
}
