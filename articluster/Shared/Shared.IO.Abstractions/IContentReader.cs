using System.Threading;
using System.Threading.Tasks;

namespace ArtiCluster.Shared.IO.Abstractions;

public interface IContentReader<T>
{
    T ReadContent()
        => ReadContentAsync().GetAwaiter().GetResult();

    Task<T> ReadContentAsync( CancellationToken cancellationToken = default );
}
