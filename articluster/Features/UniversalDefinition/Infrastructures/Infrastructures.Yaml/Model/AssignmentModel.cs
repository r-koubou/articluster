using System.Collections.Generic;

namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model;

public class AssignmentModel
{
    public MidiMessageModel MidiMessage { get; set; } = new();
    public Dictionary<string, string> Extra { get; set; } = new();
}
