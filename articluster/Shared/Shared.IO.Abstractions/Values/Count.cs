using ArtiCluster.Commons.ValueObjects;

namespace ArtiCluster.Shared.IO.Abstractions.Values;

public sealed record Count : IntValueObject
{
    public Count( int value ) : base( value )
    {
        ValueOutOfRangeException.ThrowIf( value, 0, int.MaxValue );
    }
}
