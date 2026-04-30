namespace ArtiCluster.Commons.ValueObjects;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable once UnusedType.Global
public abstract record ShortValueObject : ValueObject<short>
{
    public short MinValue { get; init; }
    public short MaxValue { get; init; }

    protected ShortValueObject(
        short value,
        short? minValue = null,
        short? maxValue = null ) : base( value )
    {
        MinValue = minValue ?? short.MinValue;
        MaxValue = maxValue ?? short.MaxValue;

        if( minValue != null || maxValue != null )
        {
            ValueOutOfRangeException.ThrowIf( Value, MinValue, MaxValue );
        }
    }
}
