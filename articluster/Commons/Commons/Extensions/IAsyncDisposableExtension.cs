using System;

namespace ArtiCluster.Commons.Extensions;

public static class IAsyncDisposableExtension
{
    public static void AddTo( this IAsyncDisposable self, CompositeAsyncDisposable target )
    {
        target.Add( self );
    }
}
