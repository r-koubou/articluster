using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtiCluster.Commons;

public sealed class CompositeAsyncDisposable : IAsyncDisposable
{
    private readonly List<IAsyncDisposable> disposables = [ ];
    private bool disposed;

    public void Add( IAsyncDisposable disposable )
    {
        ObjectDisposedException.ThrowIf( disposed, this );
        disposables.Add( disposable );
    }

    public async ValueTask DisposeAsync()
    {
        if( disposed )
        {
            return;
        }

        disposed = true;

        foreach( var x in disposables )
        {
            await x.DisposeAsync().ConfigureAwait( false );
        }

        disposables.Clear();
    }
}
