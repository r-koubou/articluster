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
        AssignmentName name,
        IReadOnlyCollection<MidiMessage>? midiMessages = null,
        IReadOnlyDictionary<string, string>? extra = null )
    {
        Name         = name;
        MidiMessages = midiMessages ?? new List<MidiMessage>();
        Extra        = extra == null ? new Dictionary<string, string>() : new Dictionary<string, string>( extra );
    }

    public static Articulation Create(
        string name,
        IReadOnlyCollection<MidiMessage>? midiMessages = null,
        IReadOnlyDictionary<string, string>? extra = null )
    {
        return new Articulation(
            new AssignmentName( name ),
            midiMessages,
            extra
        );
    }
}
