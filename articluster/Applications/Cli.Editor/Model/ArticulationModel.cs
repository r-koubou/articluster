using System.Collections.Generic;

namespace ArtiCluster.Applications.Cli.Editor.Model;

public sealed record ArticulationModel
{
    public string Name { get; set; } = string.Empty;
    public List<MidiMessageModel> MidiMessages { get; set; } = [ ];
}
