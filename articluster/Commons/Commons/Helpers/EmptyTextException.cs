using System;

namespace ArtiCluster.Commons.Helpers;

public class EmptyTextException : Exception
{
    public EmptyTextException() {}

    public EmptyTextException( string message ) : base( message ) {}
}
