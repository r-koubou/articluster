using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Commons;
using ArtiCluster.Features.Cakewalk.ArticulationMaps.Facades;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Abstractions;

using FacadeExportFailureReason = ArtiCluster.Features.Cakewalk.ArticulationMaps.Contracts.ExportFailureReason;

namespace ArtiCluster.Applications.Services.LocalFileConversions.Strategies;

public sealed class CakewalkLocalFileExportStrategy : ILocalFileExportStrategy<ProductSet>
{
    public string GetOutputDirectory( string baseDirectory, ProductSet source )
        => Path.Combine( baseDirectory, "Cakewalk", source.ManufacturerName.Value );

    public string GetExportFileName( ProductSet source )
        => source.ProductName.Value + ".artmap";

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
                }
            );
        }

        return Result<Unit, ExportFailureReason>.Success( Unit.Default );
    }
}
