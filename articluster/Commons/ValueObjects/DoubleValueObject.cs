using System;

namespace ArtiCluster.Commons.ValueObjects;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable once UnusedType.Global
public abstract record DoubleValueObject : ValueObject<double>
{
    private const double DefaultEpsilon = 1e-10;

    public static double Epsilon { get; set; }
        = DefaultEpsilon;

    public double MinValue { get; init; }
    public double MaxValue { get; init; }

    protected DoubleValueObject(
        double value,
        double? minValue = null,
        double? maxValue = null ) : base( value )
    {
        MinValue = minValue ?? double.MinValue;
        MaxValue = maxValue ?? double.MaxValue;

        if( minValue != null || maxValue != null )
        {
            ValueOutOfRangeException.ThrowIf( Value, MinValue, MaxValue );
        }
    }

    public override int GetHashCode()
        => HashCode.Combine( Value );

    public virtual bool Equals( DoubleValueObject? other )
        => other is not null && Math.Abs( Value - other.Value ) < Epsilon;
}
