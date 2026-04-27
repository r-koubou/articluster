using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinitions.Infrastructures.Yaml;
using ArtiCluster.Features.UniversalDefinitions.UseCases;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

using GatewayImportReason = ArtiCluster.Features.UniversalDefinitions.Gateways.ImportReason;
using GatewayExportReason = ArtiCluster.Features.UniversalDefinitions.Gateways.ExportReason;

namespace ArtiCluster.Features.UniversalDefinitions.Facades;

public sealed class UniversalDefinitionFacade : IUniversalDefinitionFacade
{
    public async Task<Result<UniversalDefinition, ImportReason>> ImportAsync( ITextContentReader reader, CancellationToken cancellationToken = default )
    {
        var importer = new YamlImporter();
        var useCase = new ImportUseCase();
        var input = new ImportInputPort( importer, reader );

        var result = await useCase.ExecuteAsync( input, cancellationToken );

        if( result.IsFailure )
        {
            return result.MapError( reason =>
                {
                    return reason switch
                    {
                        GatewayImportReason.DeserializationError => ImportReason.DeserializationError,
                        GatewayImportReason.IoError              => ImportReason.IoError,
                        _                                        => ImportReason.OtherError
                    };
                }
            );
        }

        return Result<UniversalDefinition, ImportReason>.Success( result.Unwrap() );
    }

    public async Task<Result<Unit, ExportReason>> ExportAsync( ITextContentWriter writer, UniversalDefinition source, CancellationToken cancellationToken = default )
    {
        var exporter = new YamlExporter();
        var useCase = new ExportUseCase();
        var input = new ExportInputPort( source, exporter, writer );

        var result = await useCase.ExecuteAsync( input, cancellationToken );

        if( result.IsFailure )
        {
            return result.MapError( reason =>
                {
                    return reason switch
                    {
                        GatewayExportReason.SerializationError => ExportReason.SerializationError,
                        GatewayExportReason.IoError            => ExportReason.IoError,
                        _                                      => ExportReason.OtherError
                    };
                }
            );
        }

        return Result<Unit, ExportReason>.Success( Unit.Default );
    }
}
