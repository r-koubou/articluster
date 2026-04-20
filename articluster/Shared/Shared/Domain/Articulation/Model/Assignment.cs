using System.Collections.Generic;

using ArtiCluster.Shared.Domain.Articulation.Model.Values.Midi;

namespace ArtiCluster.Shared.Domain.Articulation.Model;

// ReSharper disable NotAccessedPositionalProperty.Global
public sealed record Assignment(
    IReadOnlyCollection<MidiNoteNumber> MidiNoteNumbers,
    IReadOnlyCollection<MidiVelocity> MidiVelocities,
    IReadOnlyCollection<MidiCcNumber> MidiCcNumbers,
    IReadOnlyCollection<MidiCcValue> MidiCcValues,
    IReadOnlyDictionary<string, string>? Extra = null )
{
    public IReadOnlyDictionary<string, string>? Extra { get; init; }
        = Extra ?? new Dictionary<string, string>();
}
