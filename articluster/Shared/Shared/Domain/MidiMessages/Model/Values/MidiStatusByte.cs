using ArtiCluster.Commons.ValueObjects;

namespace ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

public sealed record MidiStatusByte : IntValueObject
{
    public static readonly MidiStatusByte Null = new();

    // ReSharper disable MemberCanBePrivate.Global
    public bool IsChannelVoiceMessage
        => Value is >= 0x80 and < 0xF0;

    public bool IsSystemMessage
        => Value >= 0xF0;
    // ReSharper restore MemberCanBePrivate.Global

    private MidiStatusByte() : base( -1 ) {}

    public MidiStatusByte( int value ) : base( value, minValue: 0x00, maxValue: 0xFF ) {}

    public bool TryGetChannel( out MidiChannel channel )
    {
        channel = null!;

        if( !IsChannelVoiceMessage )
        {
            return false;
        }

        channel = new MidiChannel( Value & 0x0F );

        return true;
    }
}
