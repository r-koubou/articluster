using System.Collections.Generic;

using ArtiCluster.Shared.Domain.MidiMessages.Model;

namespace ArtiCluster.Shared.Domain.Articulation.Model;

public sealed record Assignment
{
    public IReadOnlyCollection<MidiNoteOffMessage> MidiNoteOff { get; init; }
    public IReadOnlyCollection<MidiNoteOffMessage> MidiNoteOn { get; init; }
    public IReadOnlyCollection<MidiNoteOffMessage> MidiCc { get; init; }
    public IReadOnlyCollection<MidiNoteOffMessage> MidiProgramChange { get; init; }
    public IReadOnlyDictionary<string, string> Extra { get; init; }

    public Assignment(
        IReadOnlyCollection<MidiNoteOffMessage>? noteOn = null,
        IReadOnlyCollection<MidiNoteOffMessage>? noteOff = null,
        IReadOnlyCollection<MidiNoteOffMessage>? cc = null,
        IReadOnlyCollection<MidiNoteOffMessage>? programChange = null,
        IReadOnlyDictionary<string, string>? extra = null )
    {
        MidiNoteOn        = noteOn ?? [ ];
        MidiNoteOff       = noteOff ?? [ ];
        MidiCc            = cc ?? [ ];
        MidiProgramChange = programChange ?? [ ];
        Extra             = extra ?? new Dictionary<string, string>();
    }
}
