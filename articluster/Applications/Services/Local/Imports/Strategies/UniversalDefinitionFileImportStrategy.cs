using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Applications.Services.Abstractions.Imports;
using ArtiCluster.Applications.Services.Abstractions.Imports.Strategies;
using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinitions.Facades;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

using FacadeImportFailureReason = ArtiCluster.Features.UniversalDefinitions.Contracts.ImportFailureReason;

namespace ArtiCluster.Applications.Services.Local.Imports.Strategies;

public sealed class UniversalDefinitionFileImportStrategy : IImportStrategy<UniversalDefinition>
{
    public async Task<Result<UniversalDefinition, ImportFailureReason>> ImportAsync(
        ITextContentReader reader,
        CancellationToken cancellationToken = default )
    {
        var facade = new UniversalDefinitionFacade();

        var importResult = await facade.ImportAsync( reader, cancellationToken );

        if( importResult.IsFailure )
        {
            var error = importResult.UnwrapError();
            var reason = error.Reason switch
            {
                FacadeImportFailureReason.UnsupportedFormatVersion => ImportFailureReason.UnsupportedFormatVersion,
                FacadeImportFailureReason.DeserializationError     => ImportFailureReason.DeserializationError,
                FacadeImportFailureReason.IoError                  => ImportFailureReason.IoError,
                _                                                  => ImportFailureReason.OtherError
            };

            return Result<UniversalDefinition, ImportFailureReason>.Failure( reason, error.Error );
        }

        return Result<UniversalDefinition, ImportFailureReason>.Success( importResult.Unwrap() );
    }
}
