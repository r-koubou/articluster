using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtiCluster.Commons;

public sealed class CompositeDisposable : IDisposable, IAsyncDisposable
{
    private readonly List<object> disposables = [ ];

    public void Add<T>( T disposable ) where T : IDisposable, IAsyncDisposable
    {
        disposables.Add( disposable );
    }

    public void Dispose()
        => DisposeAsync().GetAwaiter().GetResult();

    public async ValueTask DisposeAsync()
    {
        foreach( var x in disposables )
        {
            switch( x )
            {
                case IAsyncDisposable asyncDisposable:
                    await asyncDisposable.DisposeAsync();

                    break;

                case IDisposable disposable:
                    disposable.Dispose();

                    break;
            }
        }

        disposables.Clear();
    }
}
