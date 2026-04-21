using ArtiCluster.Commons.Helpers;
using ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

using YamlDotNet.Serialization;

namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model.MidiMessages
{
    public sealed class MidiNoteOffMessageModel : IMidiChannelVoiceMessageModel
    {
        [YamlIgnore]
        public int Status
            => 0x80 | ( Channel & 0xF );

        [YamlMember( Alias = "Channel" )]
        public int Channel { get; set; }

        [YamlMember( Alias = "Note" )]
        // ReSharper disable once MemberCanBePrivate.Global
        public string Note { get; set; } = string.Empty;

        [YamlIgnore]
        public int Data1
        {
            get
            {
                if( string.IsNullOrEmpty( Note ) )
                {
                    return 0;
                }

                if( NumericsHelper.TryParse( Note, out var result ) ||
                    NumericsHelper.TryParse( Note, out result, 16 ) )
                {
                    return result;
                }

                return new MidiNoteName( Note ).ToMidiNoteNumber().Value;
            }
            set => Note = MidiNoteName.FromMidiNoteNumber( new MidiNoteNumber( value ) ).Value;
        }

        [YamlMember( Alias = "Velocity" )]
        public int Data2 { get; set; }

        public MidiNoteOffMessageModel() {}

        public MidiNoteOffMessageModel( int channel, int noteNumber, int velocity )
        {
            Channel = channel;
            Data1   = noteNumber;
            Data2   = velocity;
            Note    = MidiNoteName.FromMidiNoteNumber( new MidiNoteNumber( noteNumber ) ).Value;
        }
    }
}
