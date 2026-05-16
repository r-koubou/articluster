using System.Collections.Generic;

namespace ArtiCluster.Features.UniversalDefinitions.v1.Models;

/// <summary>
/// Represents an articulation, which is a specific way of playing a note (e.g., staccato, legato, etc.) that can be triggered by MIDI messages.
/// </summary>
/// <remarks>
/// Added Format Version: 1.0.0
/// </remarks>
public class ArticulationModel
{
    /// <summary>
    /// Name of the articulation (e.g., "Staccato", "Legato", etc.)
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
    public List<MidiMessageModel> MidiMessages { get; set; } = [ ];

    /// <summary>
    /// Reserved for future use. Can be used to store additional metadata as key-value pairs.
    /// </summary>
    /// <remarks>
    /// Added Format Version: 1.0.0
    /// </remarks>
    public Dictionary<string, string> Extra { get; set; } = new();
}
