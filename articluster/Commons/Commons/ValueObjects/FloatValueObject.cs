using System;

namespace ArtiCluster.Commons.ValueObjects;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable once UnusedType.Global
public abstract record FloatValueObject : ValueObject<float>
{
    private const float DefaultEpsilon = 1e-10f;

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public static float Epsilon { get; set; }
        = DefaultEpsilon;

    public float MinValue { get; init; }
    public float MaxValue { get; init; }

    protected FloatValueObject(
        float value,
        float? minValue = null,
        float? maxValue = null ) : base( value )
    {
        MinValue = minValue ?? float.MinValue;
        MaxValue = maxValue ?? float.MaxValue;

        if( minValue != null || maxValue != null )
        {
            ValueOutOfRangeException.ThrowIf( Value, MinValue, MaxValue );
        }
    }

    public override int GetHashCode()
        => HashCode.Combine( Value );

    public virtual bool Equals( FloatValueObject? other )
        => other is not null && Math.Abs( Value - other.Value ) < Epsilon;
}
