using System;

namespace ArtiCluster.Commons.Extensions;

public static class IAsyncDisposableExtension
{
    public static void AddTo( this IAsyncDisposable self, CompositeDisposable compositeDisposable )
    {
        compositeDisposable.Add( self );
    }
}
