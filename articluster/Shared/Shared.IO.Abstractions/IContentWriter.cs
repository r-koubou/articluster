using System.Threading;
using System.Threading.Tasks;

namespace ArtiCluster.Shared.IO.Abstractions;

public interface IContentWriter<in T>
{
    Task WriteAsync( T content, CancellationToken cancellationToken = default );
}
