namespace ArtiCluster.Commons.ValueObjects;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable once UnusedType.Global
public abstract record LongValueObject : ValueObject<long>
{
    public long MinValue { get; init; }
    public long MaxValue { get; init; }

    protected LongValueObject(
        long value,
        long? minValue = null,
        long? maxValue = null ) : base( value )
    {
        MinValue = minValue ?? long.MinValue;
        MaxValue = maxValue ?? long.MaxValue;

        if( minValue != null || maxValue != null )
        {
            ValueOutOfRangeException.ThrowIf( Value, MinValue, MaxValue );
        }
    }
}
