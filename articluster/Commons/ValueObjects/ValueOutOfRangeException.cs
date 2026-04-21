using System;

namespace ArtiCluster.Commons.ValueObjects;

public class ValueOutOfRangeException : Exception
{
    public ValueOutOfRangeException( string message ) : base( message ) {}

    private ValueOutOfRangeException( object actual, string message )
        : base( $"{message} (={actual})" ) {}

    // ReSharper disable once MemberCanBePrivate.Global
    public static void ThrowIf<T>( ValueObject<T> value, T min, T max ) where T : IComparable<T>
    {
        if( value.Value.CompareTo( min ) < 0 )
        {
            throw new ValueOutOfRangeException( value, $"{nameof( value )}({value}) < {nameof( min )}({min})" );
        }

        if( value.Value.CompareTo( max ) > 0 )
        {
            throw new ValueOutOfRangeException( value, $"{nameof( value )}({value}) > {nameof( max )}({max})" );
        }
    }
}
