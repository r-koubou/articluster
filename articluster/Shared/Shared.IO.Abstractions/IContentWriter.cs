using System;
using System.Threading;
using System.Threading.Tasks;

namespace ArtiCluster.Shared.IO.Abstractions;

public interface IContentWriter<in T> : IDisposable
{
    void WriteContent( T content )
        => WriteContentAsync( content ).GetAwaiter().GetResult();

    Task WriteContentAsync( T content, CancellationToken cancellationToken = default );
}
