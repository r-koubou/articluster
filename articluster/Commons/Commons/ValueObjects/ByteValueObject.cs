namespace ArtiCluster.Commons.ValueObjects;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable once UnusedType.Global
public abstract record ByteValueObject : ValueObject<byte>
{
    public byte MinValue { get; init; }
    public byte MaxValue { get; init; }

    protected ByteValueObject(
        byte value,
        byte? minValue = null,
        byte? maxValue = null ) : base( value )
    {
        MinValue = minValue ?? byte.MinValue;
        MaxValue = maxValue ?? byte.MaxValue;

        if( minValue != null || maxValue != null )
        {
            ValueOutOfRangeException.ThrowIf( Value, MinValue, MaxValue );
        }
    }
}
