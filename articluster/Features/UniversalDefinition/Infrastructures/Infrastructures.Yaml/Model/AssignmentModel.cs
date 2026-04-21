using System.Collections.Generic;

using ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model.MidiMessages;

namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model;

public class AssignmentModel
{
    public string Name { get; set; } = string.Empty;
    public List<IMidiMessageModel> NoteOn { get; set; } = [ ];
    public List<IMidiMessageModel> NoteOff { get; set; } = [ ];
    public List<IMidiMessageModel> ControlChange { get; set; } = [ ];
    public List<IMidiMessageModel> ProgramChange { get; set; } = [ ];
    public Dictionary<string, string> Extra { get; set; } = new();
}
