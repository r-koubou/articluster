using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.Facades;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Abstractions;

using ExportReason = ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.Facades.ExportReason;

namespace ArtiCluster.Applications.Services.LocalFileExporting;

public sealed class CakewalkLocalFileExportStrategy : ILocalFileExportStrategy<UniversalDefinitionProductSet>
{
    public string GetOutputDirectory( string baseDirectory, UniversalDefinitionProductSet source )
        => Path.Combine( baseDirectory, "Cakewalk", source.ManufacturerName.Value, source.ProductName.Value );

    public string GetExportFileName( UniversalDefinitionProductSet source )
        => source.ProductName.Value + ".artmap";

    public async Task<Result<Unit, ExportFailureReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinitionProductSet source,
        CancellationToken cancellationToken = default )
    {
        var facade = new CakewalkDefinitionFacade();

        var exportResult = await facade.ExportAsync( writer, source, cancellationToken );

        if( exportResult.IsFailure )
        {
            return Result<Unit, ExportFailureReason>.Failure(
                exportResult.Reason switch
                {
                    ExportReason.SerializationError => ExportFailureReason.SerializationError,
                    ExportReason.IoError            => ExportFailureReason.IoError,
                    _                               => ExportFailureReason.OtherError
                }
            );
        }

        return Result<Unit, ExportFailureReason>.Success( Unit.Default );
    }
}
