using System;

namespace ArtiCluster.Commons.Extensions;

public static class IDisposableExtension
{
    public static void AddTo( this IDisposable self, CompositeDisposable target )
    {
        target.Add( self );
    }
}
