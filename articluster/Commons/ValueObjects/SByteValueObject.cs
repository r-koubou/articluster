namespace ArtiCluster.Commons.ValueObjects;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable once UnusedType.Global
public abstract record SByteValueObject : ValueObject<sbyte>
{
    public sbyte MinValue { get; init; }
    public sbyte MaxValue { get; init; }

    protected SByteValueObject(
        sbyte value,
        sbyte? minValue = null,
        sbyte? maxValue = null ) : base( value )
    {
        MinValue = minValue ?? sbyte.MinValue;
        MaxValue = maxValue ?? sbyte.MaxValue;

        if( minValue != null || maxValue != null )
        {
            ValueOutOfRangeException.ThrowIf( Value, MinValue, MaxValue );
        }
    }
}
