using System.Collections.Generic;

namespace ArtiCluster.Features.UniversalDefinitionsNew.Models;

public class ArticulationModel
{
    public string Name { get; set; } = string.Empty;
    public List<MidiMessageModel> MidiMessages { get; set; } = [ ];
    public Dictionary<string, string> Extra { get; set; } = new();
}
