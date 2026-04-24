using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.StudioOne.Gateways;

public enum ExportReason
{
    EmptyDefinitionsError,
    MixedPatchDefinitionsError,
    SerializationError,
    IoError,
    OtherError
}

public interface IDefinitionExporter
{
    Task<Result<Unit, ExportReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinition source,
        CancellationToken cancellationToken = default
    );

    Task<Result<Unit, ExportReason>> BulkExportAsync(
        ITextContentWriter writer,
        IReadOnlyCollection<UniversalDefinition> sources,
        CancellationToken cancellationToken = default
    );
}
