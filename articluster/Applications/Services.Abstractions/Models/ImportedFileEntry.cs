using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Applications.Services.Abstractions.Models;

public sealed record ImportedFileEntry(
    string FilePath,
    UniversalDefinition Definition
);
