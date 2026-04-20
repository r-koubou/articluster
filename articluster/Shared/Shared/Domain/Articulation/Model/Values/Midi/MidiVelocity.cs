namespace ArtiCluster.Shared.Domain.Articulation.Model.Values.Midi;

public sealed record MidiVelocity( int Value ) : MidiMessageByte( Value );
