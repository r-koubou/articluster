using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Commons;
using ArtiCluster.Features.Cubase.ExpressionMaps.Facades;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Abstractions;

using FacadeExportFailureReason = ArtiCluster.Features.Cubase.ExpressionMaps.Contracts.ExportFailureReason;

namespace ArtiCluster.Applications.Services.LocalFileConversions.Strategies;

public sealed class CubaseLocalFileExportStrategy : ILocalFileExportStrategy<SeparatedArticulationGroupSet>
{
    public string GetOutputDirectory( string baseDirectory, SeparatedArticulationGroupSet source )
        => Path.Combine( baseDirectory, "Cubase", source.ManufacturerName.Value, source.ProductName.Value, source.PatchName.Value );

    public string GetExportFileName( SeparatedArticulationGroupSet source )
        => $"{source.ArticulationGroupName.Value}.expressionmap";

    public async Task<Result<Unit, ExportFailureReason>> ExportAsync(
        ITextContentWriter writer,
        SeparatedArticulationGroupSet source,
        CancellationToken cancellationToken = default )
    {
        var facade = new CubaseDefinitionFacade();

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
