using System;
using System.Collections.Generic;

namespace ArtiCluster.Commons;

public sealed class CompositeDisposable : IDisposable
{
    private readonly List<IDisposable> disposables = [ ];
    private bool disposed;

    public void Add( IDisposable disposable )
    {
        ObjectDisposedException.ThrowIf( disposed, this );
        disposables.Add( disposable );
    }

    public void Dispose()
    {
        if( disposed )
        {
            return;
        }

        disposed = true;

        foreach( var x in disposables )
        {
            x.Dispose();
        }

        disposables.Clear();
    }
}
