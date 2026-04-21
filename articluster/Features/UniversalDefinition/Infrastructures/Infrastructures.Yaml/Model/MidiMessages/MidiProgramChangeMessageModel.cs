using YamlDotNet.Serialization;

namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model.MidiMessages
{
    public class MidiProgramChangeMessageModel : IMidiChannelVoiceMessageModel
    {
        [YamlIgnore]
        public int Status => 0xC0 | ( Channel & 0xF );

        [YamlMember( Alias = "Channel" )]
        public int Channel { get; set; }

        [YamlMember( Alias = "ProgramNumber" )]
        public int Data1 { get; set; }

        [YamlIgnore]
        public int Data2 { get; set; } = 0x00;

        public MidiProgramChangeMessageModel() {}

        public MidiProgramChangeMessageModel( int channel, int programNumber )
        {
            Channel = channel;
            Data1   = programNumber;
            Data2   = 0x00;
        }
    }
}
