using ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

namespace ArtiCluster.Shared.Domain.MidiMessages.Model;

public sealed record MidiMessage
{
    public MidiStatusByte Status { get; init; }
    public MidiDataByte Data1 { get; init; }
    public MidiDataByte Data2 { get; init; }

    public MidiMessage( int statusByte, int? dataByte1 = null, int? dataByte2 = null )
    {
        Status = new MidiStatusByte( statusByte );
        Data1  = dataByte1 != null ? new MidiDataByte( dataByte1.Value ) : MidiDataByte.None;
        Data2  = dataByte2 != null ? new MidiDataByte( dataByte2.Value ) : MidiDataByte.None;
    }
}
