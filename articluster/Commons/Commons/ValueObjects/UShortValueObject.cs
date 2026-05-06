namespace ArtiCluster.Commons.ValueObjects;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable once UnusedType.Global
public abstract record UShortValueObject : ValueObject<ushort>
{
    public ushort MinValue { get; init; }
    public ushort MaxValue { get; init; }

    protected UShortValueObject(
        ushort value,
        ushort? minValue = null,
        ushort? maxValue = null ) : base( value )
    {
        MinValue = minValue ?? ushort.MinValue;
        MaxValue = maxValue ?? ushort.MaxValue;

        if( minValue != null || maxValue != null )
        {
            ValueOutOfRangeException.ThrowIf( Value, MinValue, MaxValue );
        }
    }
}
