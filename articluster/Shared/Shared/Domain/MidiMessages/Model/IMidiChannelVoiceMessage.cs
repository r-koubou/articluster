using ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

namespace ArtiCluster.Shared.Domain.MidiMessages.Model;

public interface IMidiChannelVoiceMessage : IMidiMessage
{
    public MidiChannel Channel { get; }
}
