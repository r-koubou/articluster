using System;
using System.Collections.Generic;

namespace ArtiCluster.Applications.Cli.Editor.Model;

public sealed record UniversalDefinitionModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Author { get; init; } = string.Empty;
    public string ManufacturerName { get; init; } = string.Empty;
    public string ProductName { get; init; } = string.Empty;
    public string PatchName { get; init; } = string.Empty;
    public List<ArticulationModel> Articulations { get; init; } = [ ];
    public Dictionary<string, string> Extra { get; init; } = new();
}
