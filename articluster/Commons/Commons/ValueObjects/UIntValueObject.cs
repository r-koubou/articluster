namespace ArtiCluster.Commons.ValueObjects;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable once UnusedType.Global
public abstract record UIntValueObject : ValueObject<uint>
{
    public uint MinValue { get; init; }
    public uint MaxValue { get; init; }

    protected UIntValueObject(
        uint value,
        uint? minValue = null,
        uint? maxValue = null ) : base( value )
    {
        MinValue = minValue ?? uint.MinValue;
        MaxValue = maxValue ?? uint.MaxValue;

        if( minValue != null || maxValue != null )
        {
            ValueOutOfRangeException.ThrowIf( Value, MinValue, MaxValue );
        }
    }
}
