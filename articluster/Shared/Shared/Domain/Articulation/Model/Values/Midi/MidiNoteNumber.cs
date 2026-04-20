namespace ArtiCluster.Shared.Domain.Articulation.Model.Values.Midi;

public sealed record MidiNoteNumber( int Value ) : MidiMessageByte( Value );
