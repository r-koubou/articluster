using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.Cubase.ExpressionMapDefinitions.Facades;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Applications.Services.LocalFileExporting;

public sealed class CubaseLocalFileExportStrategy : ILocalFileExportStrategy<UniversalDefinition>
{
    public string GetOutputDirectory( string baseDirectory, UniversalDefinition source )
        => Path.Combine( baseDirectory, "Cubase", source.ManufacturerName.Value, source.ProductName.Value );

    public string GetExportFileName( UniversalDefinition source )
        => source.ProductName.Value + ".expressionmap";

    public async Task<Result<Unit, ExportFailureReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinition source,
        CancellationToken cancellationToken = default )
    {
        var facade = new CubaseDefinitionFacade();

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
