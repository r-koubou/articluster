using System;
using System.Collections.Generic;

using Semver;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CollectionNeverQueried.Global

namespace ArtiCluster.Features.UniversalDefinitions.v1.Models;

public class UniversalDefinitionModel
{
    public static readonly SemVersion CurrentFormatVersion = new( 1, 0, 0 );

    public string FormatVersion { get; set; } = CurrentFormatVersion.ToString();

    public Guid Id { get; set; } = Guid.NewGuid();

    public string Author { get; set; } = string.Empty;

    public string ManufacturerName { get; set; } = string.Empty;

    public string ProductName { get; set; } = string.Empty;

    public string PatchName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<ArticulationModel> Articulations { get; set; } = new();

    public Dictionary<string, string> Extra { get; set; } = new();
}
