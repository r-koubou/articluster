using System.Collections.Generic;

namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model;

internal class AssignmentModel
{
    public string Name { get; set; } = string.Empty;
    public List<MidiMessageModel> MidiMessages { get; set; } = [ ];
    public Dictionary<string, string> Extra { get; set; } = new();
}
