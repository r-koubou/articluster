using System;

namespace ArtiCluster.Commons.Helpers;

public static class StringHelper
{
    private static bool IsEmptyImpl( string text )
        => string.IsNullOrEmpty( text )
           || text == string.Empty
           || text.Trim().Length == 0;

    public static bool IsEmpty( params string[] texts )
    {
        foreach( var t in texts )
        {
            if( IsEmptyImpl( t ) )
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsEmpty( string text )
        => IsEmptyImpl( text );

    public static bool IsEmpty( object obj )
        => obj == null!
           || IsEmpty( obj.ToString()! );

    public static void ValidateEmpty( string text )
    {
        ValidateEmpty<EmptyTextException>( text );
    }

    public static void ValidateEmpty<TException>( string text ) where TException : Exception, new()
    {
        if( IsEmpty( text ) )
        {
            throw new TException();
        }
    }

    public static bool IsNull( string text )
    {
        return text == null!;
    }

    public static void ValidateNull( string text )
    {
        ValidateNull<ArgumentNullException>( text );
    }

    public static void ValidateNull<TException>( string text ) where TException : Exception, new()
    {
        if( IsNull( text ) )
        {
            throw new TException();
        }
    }

    public static bool Contains( string search, params string[] texts )
    {
        if( IsEmptyImpl( search ) )
        {
            return false;
        }

        foreach( var i in texts )
        {
            if( i.Contains( search ) )
            {
                return true;
            }
        }

        return false;
    }
}
