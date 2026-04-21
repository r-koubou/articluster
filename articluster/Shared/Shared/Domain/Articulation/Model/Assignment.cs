using System.Collections.Generic;

using ArtiCluster.Shared.Domain.Articulation.Model.Values;
using ArtiCluster.Shared.Domain.MidiMessages.Model;

namespace ArtiCluster.Shared.Domain.Articulation.Model;

public sealed record Assignment
{
    public AssignmentName Name { get; init; }
    public IReadOnlyCollection<MidiNoteOnMessage> MidiNoteOn { get; init; }
    public IReadOnlyCollection<MidiNoteOffMessage> MidiNoteOff { get; init; }
    public IReadOnlyCollection<MidiControlChangeMessage> MidiControlChange { get; init; }
    public IReadOnlyCollection<MidiProgramChangeMessage> MidiProgramChange { get; init; }
    public IReadOnlyDictionary<string, string> Extra { get; init; }

    public Assignment(
        string name,
        IReadOnlyCollection<MidiNoteOnMessage>? noteOn = null,
        IReadOnlyCollection<MidiNoteOffMessage>? noteOff = null,
        IReadOnlyCollection<MidiControlChangeMessage>? controlChange = null,
        IReadOnlyCollection<MidiProgramChangeMessage>? programChange = null,
        IReadOnlyDictionary<string, string>? extra = null )
    {
        Name              = new AssignmentName( name );
        MidiNoteOn        = noteOn ?? [ ];
        MidiNoteOff       = noteOff ?? [ ];
        MidiControlChange            = controlChange ?? [ ];
        MidiProgramChange = programChange ?? [ ];
        Extra             = extra ?? new Dictionary<string, string>();
    }
}
