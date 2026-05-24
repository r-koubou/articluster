using System.Collections.Generic;

using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Applications.Services.Abstractions.Executors;

public interface IUniversalDefinitionImportExecutor
    : IImportExecutor<IReadOnlyCollection<UniversalDefinition>>;
