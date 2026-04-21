namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model.MidiMessages
{
    public interface IMidiChannelVoiceMessageModel : IMidiMessageModel
    {
        public int Channel { get; set; }
    }
}
