using System.Collections.Generic;

using ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model.MidiMessages;

namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model;

public class AssignmentModel
{
    public string Name { get; set; } = string.Empty;
    public List<MidiNoteOnMessageModel> NoteOn { get; set; } = [ ];
    public List<MidiNoteOffMessageModel> NoteOff { get; set; } = [ ];
    public List<MidiControlChangeMessageModel> ControlChange { get; set; } = [ ];
    public List<MidiProgramChangeMessageModel> ProgramChange { get; set; } = [ ];
    public Dictionary<string, string> Extra { get; set; } = new();
}
