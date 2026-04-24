namespace ArtiCluster.Commons.ValueObjects;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable once UnusedType.Global
public abstract record ULongValueObject : ValueObject<ulong>
{
    public ulong MinValue { get; init; }
    public ulong MaxValue { get; init; }

    protected ULongValueObject(
        ulong value,
        ulong? minValue = null,
        ulong? maxValue = null ) : base( value )
    {
        MinValue = minValue ?? ulong.MinValue;
        MaxValue = maxValue ?? ulong.MaxValue;

        if( minValue != null || maxValue != null )
        {
            ValueOutOfRangeException.ThrowIf( Value, MinValue, MaxValue );
        }
    }
}
