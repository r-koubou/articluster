using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.Abstractions.Strategies;
using ArtiCluster.Applications.Services.LocalFileConversions.Strategies;
using ArtiCluster.Commons;

namespace ArtiCluster.Applications.Services.LocalFileConversions.Runners;

public interface ILocalFileConversionRunner
{
    Task<Result<Unit, ConvertFailureReason>> RunAsync<TSource>(
        string outputBaseDirectory,
        IEnumerable<TSource> sources,
        ILocalOutputNamingStrategy<TSource> namingStrategy,
        ILocalFileExportStrategy<TSource> strategy,
        CancellationToken cancellationToken = default );
}
