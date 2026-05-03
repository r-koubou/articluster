using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions;
using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinitions.Contracts;
using ArtiCluster.Features.UniversalDefinitions.Facades;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Local;

namespace ArtiCluster.Applications.Services;

public sealed class BulkImportUniversalDefinitionService : IBulkImportUniversalDefinitionService
{
    public async Task<Result<UniversalDefinitionProductCollection, BulkImportFailureReason>> ImportAsync( string definitionsDirectory, CancellationToken cancellationToken = default )
    {
        try
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
                        ImportFailureReason.DeserializationError => BulkImportFailureReason.DeserializationError,
                        ImportFailureReason.IoError              => BulkImportFailureReason.IoError,
                        _                                        => BulkImportFailureReason.OtherError
                    };

                    return Result<UniversalDefinitionProductCollection, BulkImportFailureReason>.Failure( reason );
                }

                definitions.Add( importResult.Unwrap() );
            }

            return Result<UniversalDefinitionProductCollection, BulkImportFailureReason>.Success(
                new UniversalDefinitionProductCollection( definitions )
            );
        }
        catch( IOException e )
        {
            return Result<UniversalDefinitionProductCollection, BulkImportFailureReason>.Failure(
                BulkImportFailureReason.IoError,
                e
            );
        }
        catch( Exception e )
        {
            return Result<UniversalDefinitionProductCollection, BulkImportFailureReason>.Failure(
                BulkImportFailureReason.OtherError,
                e
            );
        }
    }
}
