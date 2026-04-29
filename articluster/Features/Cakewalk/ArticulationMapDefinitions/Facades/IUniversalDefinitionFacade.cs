using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.Facades;

public enum ExportReason
{
    SerializationError,
    IoError,
    OtherError
}

public interface IStudioOneDefinitionFacade
{
    public Task<Result<Unit, ExportReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinitionProductSet source,
        CancellationToken cancellationToken = default );

    public Task<Result<Unit, ExportReason>> ExportAsync(
        string exportDirectory,
        UniversalDefinitionProductSet source,
        CancellationToken cancellationToken = default );
}
