using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.Logic.Articulations.Contracts;
using ArtiCluster.Features.Logic.Articulations.Exports;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.Logic.Articulations.Facades;

public sealed class LogicDefinitionFacade : ILogicDefinitionFacade
{
    public async Task<Result<Unit, ExportFailureReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinition source,
        CancellationToken cancellationToken = default )
    {
        var exporter = new LogicExporter();
        var result = await exporter.ExportAsync( writer, source, cancellationToken );

        return result.IsSuccess
            ? Result<Unit, ExportFailureReason>.Success( Unit.Default )
            : result;
    }
}
