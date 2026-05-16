using System.Collections.Generic;

namespace ArtiCluster.Features.UniversalDefinitions.v1.Models;

/// <summary>
/// Represents an articulation group.
/// </summary>
/// <remarks>
/// Added Format Version: 1.0.0
/// </remarks>
public class ArticulationGroupModel
{
    /// <summary>
    /// Name of the articulation group (e.g., "Main", "FX", etc.)
    /// </summary>
    /// <remarks>
    /// Added Format Version: 1.0.0
    /// </remarks>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// MIDI messages that trigger this articulation.
    /// </summary>
    /// <remarks>
    /// Added Format Version: 1.0.0
    /// </remarks>
    public List<ArticulationModel> Articulations { get; set; } = [ ];

    /// <summary>
    /// Reserved for future use. Can be used to store additional metadata as key-value pairs.
    /// </summary>
    /// <remarks>
    /// Added Format Version: 1.0.0
    /// </remarks>
    public Dictionary<string, string> Extra { get; set; } = new();
}
