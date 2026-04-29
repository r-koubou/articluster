using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.Logic.ArticulationSetDefinitions.Facades;

public enum ExportReason
{
    SerializationError,
    IoError,
    OtherError
}

public interface ILogicDefinitionFacade
{
    public Task<Result<Unit, ExportReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinition source,
        CancellationToken cancellationToken = default );

    public Task<Result<Unit, ExportReason>> ExportAsync(
        string exportDirectory,
        UniversalDefinition source,
        CancellationToken cancellationToken = default );
}
