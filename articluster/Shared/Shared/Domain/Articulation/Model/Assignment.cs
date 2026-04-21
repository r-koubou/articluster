using System.Collections.Generic;

namespace ArtiCluster.Shared.Domain.Articulation.Model;

public sealed record Assignment
{
    public MidiMessage MidiNoteOn { get; init; }
    public MidiMessage MidiNoteOff { get; init; }
    public MidiMessage MidiCc { get; init; }
    public IReadOnlyDictionary<string, string> Extra { get; init; }

    public Assignment(
        MidiMessage? noteOn = null,
        MidiMessage? noteOff = null,
        MidiMessage? cc = null,
        IReadOnlyDictionary<string, string>? extra = null )
    {
        MidiNoteOn  = noteOn ?? MidiMessage.Null;
        MidiNoteOff = noteOff ?? MidiMessage.Null;
        MidiCc      = cc ?? MidiMessage.Null;
        Extra       = extra ?? new Dictionary<string, string>();
    }
}
