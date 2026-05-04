namespace ArtiCluster.Applications.Cli.Editor.Model;

public sealed record MidiMessageModel
{
    public int Status { get; set; }
    public int Data1 { get; set; }
    public int Data2 { get; set; }
}
