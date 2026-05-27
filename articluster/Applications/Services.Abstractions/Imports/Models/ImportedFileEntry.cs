using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Applications.Services.Abstractions.Imports.Models;

public sealed record ImportedFileEntry(
    string FilePath,
    UniversalDefinition Definition
);
