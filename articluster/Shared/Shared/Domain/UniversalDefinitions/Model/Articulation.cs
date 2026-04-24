using System.Collections.Generic;

using ArtiCluster.Shared.Domain.MidiMessages.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model.Values;

namespace ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

public sealed record Articulation
{
    public AssignmentName Name { get; init; }
    public IReadOnlyCollection<MidiMessage> MidiMessages { get; init; }
    public IReadOnlyDictionary<string, string> Extra { get; init; }

    public Articulation(
        string name,
        IReadOnlyCollection<MidiMessage>? midiMessages = null,
        IReadOnlyDictionary<string, string>? extra = null )
    {
        Name         = new AssignmentName( name );
        MidiMessages = midiMessages ?? new List<MidiMessage>();
        Extra        = extra == null ? new Dictionary<string, string>() : new Dictionary<string, string>( extra );
    }
}
