using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.LocalFileExporting;
using ArtiCluster.Commons;

namespace ArtiCluster.Applications.Services.LocalFileConversions;

public interface ILocalFileConversionRunner
{
    Task<Result<Unit, ConvertReason>> RunAsync<TSource>(
        string outputBaseDirectory,
        IEnumerable<TSource> sources,
        ILocalFileExportStrategy<TSource> strategy,
        CancellationToken cancellationToken = default );
}
