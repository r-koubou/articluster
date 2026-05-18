using ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

namespace ArtiCluster.Features.UniversalDefinitions.Models;

/// <summary>
/// Represents a MIDI message, which can be used to define the MIDI message that triggers an articulation or a parameter change.
/// </summary>
public sealed class MidiMessageModel
{
    // ReSharper disable PropertyCanBeMadeInitOnly.Global
    public int? Channel { get; init; } = MidiChannel.None.Value;
    public int Status { get; init; }
    public int? Data1 { get; init; }
    public int? Data2 { get; init; }
    // ReSharper restore PropertyCanBeMadeInitOnly.Global

    public MidiMessageModel() {}

    public MidiMessageModel( int status, int? data1 = null, int? data2 = null, int? channel = null )
    {
        Status  = status;
        Data1   = data1;
        Data2   = data2;
        Channel = channel;
    }
}
