using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtiCluster.Commons;

public sealed class CompositeDisposable : IDisposable, IAsyncDisposable
{
    private readonly List<IDisposable> disposables = [ ];
    private readonly List<IAsyncDisposable> asyncDisposables = [ ];

    public void Add( IDisposable disposable )
    {
        disposables.Add( disposable );
    }

    public void Add( IAsyncDisposable disposable )
    {
        asyncDisposables.Add( disposable );
    }

    public void Dispose()
    {
        foreach( var x in disposables )
        {
            try
            {
                x.Dispose();
            }
            catch
            {
                // ignored
            }
        }

        disposables.Clear();
    }

    public async ValueTask DisposeAsync()
    {
        foreach( var x in asyncDisposables )
        {
            try
            {
                await x.DisposeAsync();
            }
            catch
            {
                // ignored
            }
        }

        asyncDisposables.Clear();
    }
}
