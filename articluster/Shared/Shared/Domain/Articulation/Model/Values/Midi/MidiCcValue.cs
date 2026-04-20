namespace ArtiCluster.Shared.Domain.Articulation.Model.Values.Midi;

public sealed record MidiCcValue( int Value ) : MidiMessageByte( Value );
