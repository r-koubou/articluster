using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinitions.Facades;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Local;

namespace ArtiCluster.Applications.Services;

public sealed class BulkImportUniversalDefinitionService : IBulkImportUniversalDefinitionService
{
    public async Task<Result<UniversalDefinitionProductCollection, BulkImportReason>> ImportAsync( string definitionsDirectory, CancellationToken cancellationToken = default )
    {
        var facade = new UniversalDefinitionFacade();
        var definitionFiles = Directory.GetFiles( definitionsDirectory, "*.yaml", SearchOption.AllDirectories );
        var definitions = new List<UniversalDefinition>();

        foreach( var file in definitionFiles )
        {
            using var reader = new LocalTextContentReader( file );
            var importResult = await facade.ImportAsync( reader, cancellationToken );

            if( importResult.IsFailure )
            {
                var reason = importResult.Reason switch
                {
                    ImportReason.DeserializationError => BulkImportReason.DeserializationError,
                    ImportReason.IoError              => BulkImportReason.IoError,
                    _                                 => BulkImportReason.OtherError
                };

                return Result<UniversalDefinitionProductCollection, BulkImportReason>.Failure( reason );
            }

            definitions.Add( importResult.Unwrap() );
        }

        return Result<UniversalDefinitionProductCollection, BulkImportReason>.Success(
            new UniversalDefinitionProductCollection( definitions )
        );
    }
}
