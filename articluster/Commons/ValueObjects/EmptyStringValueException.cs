using System;

namespace ArtiCluster.Commons.ValueObjects;

public class EmptyStringValueException : Exception
{
    private EmptyStringValueException() {}

    public EmptyStringValueException( string message )
        : base( message ) {}

    public static void ThrowIfEmpty( StringValueObject value )
    {
        if( !value.AllowEmpty && string.IsNullOrEmpty( value.Value ) )
        {
            throw new EmptyStringValueException( $"{value.GetType().FullName} cannot be empty." );
        }
    }
}
