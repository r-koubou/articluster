using ArtiCluster.Shared.Domain.Articulation.Model.Values.Midi;

namespace ArtiCluster.Shared.Domain.Articulation.Model;

public record MidiMessage
{
    public static readonly MidiMessage Null
        = new( MidiStatusByte.Null, MidiMessageByte.Null, MidiMessageByte.Null );

    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public MidiStatusByte Status { get; init; }
    public MidiMessageByte DataByte1 { get; init; }
    public MidiMessageByte DataByte2 { get; init; }
    // ReSharper restore UnusedAutoPropertyAccessor.Global

    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable once MemberCanBePrivate.Global
    public MidiMessage( MidiStatusByte statusByte, MidiMessageByte dataByte1, MidiMessageByte dataByte2 )
    {
        Status    = statusByte;
        DataByte1 = dataByte1;
        DataByte2 = dataByte2;
    }
}
