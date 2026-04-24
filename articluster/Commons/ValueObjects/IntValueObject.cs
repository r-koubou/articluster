namespace ArtiCluster.Commons.ValueObjects;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable once UnusedType.Global
public abstract record IntValueObject : ValueObject<int>
{
    public int MinValue { get; init; }
    public int MaxValue { get; init; }

    protected IntValueObject(
        int value,
        int? minValue = null,
        int? maxValue = null ) : base( value )
    {
        MinValue = minValue ?? int.MinValue;
        MaxValue = maxValue ?? int.MaxValue;

        if( minValue != null || maxValue != null )
        {
            ValueOutOfRangeException.ThrowIf( Value, MinValue, MaxValue );
        }
    }
}
