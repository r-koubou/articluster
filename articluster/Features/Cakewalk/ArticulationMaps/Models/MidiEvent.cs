using System.Text.Json.Serialization;

namespace ArtiCluster.Features.Cakewalk.ArticulationMaps.Models
{
    public enum PlayAt
    {
        Start = 1,
        End,
        Both,
        Duration,
        Default = Duration
    }

    public enum ChaseMode
    {
        Note = 1,
        CCs,
        Full,
        Default = CCs
    }

    public class MidiEvent
    {
        [JsonPropertyName( "b1" )]
        public int Byte1 { get; set; } = 0x00;

        [JsonPropertyName( "b2" )]
        public int Byte2 { get; set; } = 0x00;

        [JsonPropertyName( "b3" )]
        public int Byte3 { get; set; } = 0x00;

        [JsonPropertyName( "b4" )]
        public int Byte4 { get; set; } = 0x00;

        [JsonPropertyName( "allowTranspose" )]
        // 0: off
        // 1: on
        public int AllowTranspose { get; set; } = 0;

        [JsonPropertyName( "allowTransposeMidiCh" )]
        // midi ch in status byte is     0: 1
        // midi ch in status byte is not 0: 0
        public int AllowTransposeMidiCh { get; set; } = 1;

        [JsonPropertyName( "triggerAt" )]
        public PlayAt TriggerAt { get; set; } = PlayAt.Default;

        [JsonPropertyName( "chaseMode" )]
        public ChaseMode ChaseMode { get; set; } = ChaseMode.Default;
    }
}
