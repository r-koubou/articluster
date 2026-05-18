using System;
using System.Collections.Generic;

using ArtiCluster.Features.UniversalDefinitions.Contracts;

using YamlDotNet.Serialization;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CollectionNeverQueried.Global

namespace ArtiCluster.Features.UniversalDefinitions.Models;

/// <summary>
/// The root of articulation definition data.
/// </summary>
public class UniversalDefinitionModel : IUniversalDefinitionModel
{
    /// <summary>
    /// Current format version.
    /// </summary>
    /// <remarks>
    /// The current value of version `0` indicates that it is not yet mature and frequently involves breaking changes. We plan to update it to version 1 in the future once it reaches a stable and mature state.
    /// </remarks>
    public const int CurrentFormatVersion = 0;

    /// <summary>
    /// Format version of this definition data.
    /// </summary>
    /// <remarks>
    /// Added Format Version: 0
    /// </remarks>
    [YamlMember( Alias = IUniversalDefinitionModel.FormatVersionFieldName )]
    public int FormatVersion { get; set; }

    /// <summary>
    /// Identifier
    /// </summary>
    /// <remarks>
    /// Added Format Version: 0
    /// </remarks>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Author name
    /// </summary>
    /// <remarks>
    /// Since 1.0.0
    /// </remarks>
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// Manufacturer name of the product
    /// </summary>
    /// <remarks>
    /// Added Format Version: 0
    /// </remarks>
    public string ManufacturerName { get; set; } = string.Empty;

    /// <summary>
    /// Product name
    /// </summary>
    /// <remarks>
    /// Added Format Version: 0
    /// </remarks>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Patch name (e.g.: Guitar, Bass, etc.)
    /// </summary>
    /// <remarks>
    /// Added Format Version: 0
    /// </remarks>
    public string PatchName { get; set; } = string.Empty;

    /// <summary>
    /// Description of this definition
    /// </summary>
    /// <remarks>
    /// Added Format Version: 0
    /// </remarks>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Articulation mappings
    /// </summary>
    /// <remarks>
    /// Added Format Version: 0
    /// </remarks>
    public List<ArticulationGroupModel> ArticulationGroups { get; set; } = new();

    /// <summary>
    /// Reserved for future use. Can be used to store additional metadata as key-value pairs.
    /// </summary>
    /// <remarks>
    /// Added Format Version: 0
    /// </remarks>
    public Dictionary<string, string> Extra { get; set; } = new();
}
