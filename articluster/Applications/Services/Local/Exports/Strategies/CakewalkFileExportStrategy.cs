using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Exports;
using ArtiCluster.Applications.Services.Abstractions.Exports.Strategies;
using ArtiCluster.Commons;
using ArtiCluster.Features.Cakewalk.ArticulationMaps.Facades;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Abstractions;

using FacadeExportFailureReason = ArtiCluster.Features.Cakewalk.ArticulationMaps.Contracts.ExportFailureReason;

namespace ArtiCluster.Applications.Services.Local.Exports.Strategies;

public sealed class CakewalkFileExportStrategy : IExportStrategy<ProductSet>
{
    public async Task<Result<Unit, ExportFailureReason>> ExportAsync(
        ITextContentWriter writer,
        ProductSet source,
        CancellationToken cancellationToken = default )
    {
        var facade = new CakewalkDefinitionFacade();

        var exportResult = await facade.ExportAsync( writer, source, cancellationToken );

        if( exportResult.IsFailure )
        {
            return Result<Unit, ExportFailureReason>.Failure(
                exportResult.Reason switch
                {
                    FacadeExportFailureReason.SerializationError => ExportFailureReason.SerializationError,
                    FacadeExportFailureReason.IoError            => ExportFailureReason.IoError,
                    _                                            => ExportFailureReason.OtherError
                },
                exportResult.UnwrapError().Error
            );
        }

        return Result<Unit, ExportFailureReason>.Success( Unit.Default );
    }
}
