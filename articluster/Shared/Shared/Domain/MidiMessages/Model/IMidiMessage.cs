using ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

namespace ArtiCluster.Shared.Domain.MidiMessages.Model;

public interface IMidiMessage
{
    public MidiMessageByte StatusByte { get; }
    public MidiMessageByte DataByte1 { get; }
    public MidiMessageByte DataByte2 { get; }
}
