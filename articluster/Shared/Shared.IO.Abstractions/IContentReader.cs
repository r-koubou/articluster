using System.Threading;
using System.Threading.Tasks;

namespace ArtiCluster.Shared.IO.Abstractions;

public interface IContentReader<T>
{
    Task<T> ReadAllAsync( CancellationToken cancellationToken = default );
}
