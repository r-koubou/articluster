using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Commons;
using ArtiCluster.Features.StudioOne.KeySwitches.Facades;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

using FacadeExportFailureReason = ArtiCluster.Features.StudioOne.KeySwitches.Contracts.ExportFailureReason;

namespace ArtiCluster.Applications.Services.LocalFileConversions.Strategies;

public sealed class StudioOneLocalFileExportStrategy : ILocalFileExportStrategy<UniversalDefinition>
{
    public string GetOutputDirectory( string baseDirectory, UniversalDefinition source )
        => Path.Combine( baseDirectory, "StudioOne", source.ManufacturerName.Value, source.ProductName.Value );

    public string GetExportFileName( UniversalDefinition source )
        => source.PatchName.Value + ".keyswitch";

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
                }
            );
        }

        return Result<Unit, ExportFailureReason>.Success( Unit.Default );
    }
}
