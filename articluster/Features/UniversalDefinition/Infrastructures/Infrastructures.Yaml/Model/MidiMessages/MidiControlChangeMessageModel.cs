using YamlDotNet.Serialization;

namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model.MidiMessages
{
    public class MidiControlChangeMessageModel : IMidiChannelVoiceMessageModel
    {
        [YamlIgnore]
        public int Status
            => 0xB0 | ( Channel & 0xF );

        [YamlMember( Alias = "Channel" )]
        public int Channel { get; set; }

        [YamlMember( Alias = "ControlNumber" )]
        public int Data1 { get; set; }

        [YamlMember( Alias = "Data" )]
        public int Data2 { get; set; }

        public MidiControlChangeMessageModel() {}

        public MidiControlChangeMessageModel( int channel, int controlNumber, int controlValue )
        {
            Channel = channel;
            Data1   = controlNumber;
            Data2   = controlValue;
        }
    }
}
