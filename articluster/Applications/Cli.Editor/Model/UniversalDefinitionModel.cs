using System;
using System.Collections.Generic;

namespace ArtiCluster.Applications.Cli.Editor.Model;

public sealed record UniversalDefinitionModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Author { get; set; } = string.Empty;
    public string ManufacturerName { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string PatchName { get; set; } = string.Empty;
    public List<ArticulationModel> Articulations { get; set; } = [ ];
}
