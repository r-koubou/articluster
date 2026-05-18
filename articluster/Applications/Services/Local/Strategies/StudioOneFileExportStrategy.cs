using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Applications.Services.Abstractions.Strategies;
using ArtiCluster.Commons;
using ArtiCluster.Features.StudioOne.KeySwitches.Facades;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

using FacadeExportFailureReason = ArtiCluster.Features.StudioOne.KeySwitches.Contracts.ExportFailureReason;

namespace ArtiCluster.Applications.Services.Local.Strategies;

public sealed class StudioOneFileExportStrategy : IExportStrategy<UniversalDefinition>
{
    public async Task<Result<Unit, ExportFailureReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinition source,
        CancellationToken cancellationToken = default )
    {
        var facade = new StudioOneDefinitionFacade();

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
