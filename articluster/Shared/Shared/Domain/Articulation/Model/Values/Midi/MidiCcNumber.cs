namespace ArtiCluster.Shared.Domain.Articulation.Model.Values.Midi;

public sealed record MidiCcNumber( int Value ) : MidiMessageByte( Value );
