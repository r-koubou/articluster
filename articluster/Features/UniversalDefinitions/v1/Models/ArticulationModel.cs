using System.Collections.Generic;

namespace ArtiCluster.Features.UniversalDefinitions.v1.Models;

public class ArticulationModel
{
    public string Name { get; set; } = string.Empty;
    public List<MidiMessageModel> MidiMessages { get; set; } = [ ];
    public Dictionary<string, string> Extra { get; set; } = new();
}
