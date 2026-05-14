using System;
using System.Collections.Generic;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CollectionNeverQueried.Global

namespace ArtiCluster.Features.UniversalDefinitions.v1.Models;

public class UniversalDefinitionModel
{
    public const int CurrentFormatVersion = 1;

    public int FormatVersion { get; set; }

    public Guid Id { get; set; } = Guid.NewGuid();

    public string Author { get; set; } = string.Empty;

    public string ManufacturerName { get; set; } = string.Empty;

    public string ProductName { get; set; } = string.Empty;

    public string PatchName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<ArticulationModel> Articulations { get; set; } = new();

    public Dictionary<string, string> Extra { get; set; } = new();
}
