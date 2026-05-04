using System.Collections.Generic;

namespace ArtiCluster.Applications.Cli.Editor.Model;

public sealed record ArticulationModel
{
    public string Name { get; init; } = string.Empty;
    public List<MidiMessageModel> MidiMessages { get; init; } = [ ];
    public Dictionary<string, string> Extra { get; init; } = new();
}
