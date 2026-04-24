using System.Collections.Generic;

using ArtiCluster.Shared.Domain.Articulation.Model.Values;
using ArtiCluster.Shared.Domain.MidiMessages.Model;

namespace ArtiCluster.Shared.Domain.Articulation.Model;

public sealed record Assignment
{
    public AssignmentName Name { get; init; }
    public IReadOnlyCollection<MidiMessage> MidiMessages { get; init; }
    public IReadOnlyDictionary<string, string> Extra { get; init; }

    public Assignment(
        string name,
        IReadOnlyCollection<MidiMessage>? midiMessages = null,
        IReadOnlyDictionary<string, string>? extra = null )
    {
        Name         = new AssignmentName( name );
        MidiMessages = midiMessages ?? new List<MidiMessage>();
        Extra        = extra == null ? new Dictionary<string, string>() : new Dictionary<string, string>( extra );
    }
}
