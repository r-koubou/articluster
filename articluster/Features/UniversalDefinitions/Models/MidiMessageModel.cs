namespace ArtiCluster.Features.UniversalDefinitions.Models;

public sealed class MidiMessageModel
{
    // ReSharper disable PropertyCanBeMadeInitOnly.Global
    public int Status { get; set; }
    public int? Data1 { get; set; }
    public int? Data2 { get; set; }
    // ReSharper restore PropertyCanBeMadeInitOnly.Global

    public MidiMessageModel() {}

    public MidiMessageModel( int status, int? data1 = null, int? data2 = null )
    {
        Status = status;
        Data1  = data1;
        Data2  = data2;
    }
}
