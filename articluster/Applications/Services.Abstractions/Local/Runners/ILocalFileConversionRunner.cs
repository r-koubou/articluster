using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Local.Strategies;
using ArtiCluster.Commons;

namespace ArtiCluster.Applications.Services.Abstractions.Local.Runners;

public interface ILocalFileConversionRunner
{
    Task<Result<Unit, ConvertFailureReason>> RunAsync<TSource>(
        string outputBaseDirectory,
        IEnumerable<TSource> sources,
        ILocalOutputNamingStrategy<TSource> namingStrategy,
        ILocalFileExportStrategy<TSource> strategy,
        CancellationToken cancellationToken = default );
}
