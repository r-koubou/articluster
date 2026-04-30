using System.Collections.Generic;

using ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

namespace ArtiCluster.Shared.Domain.MidiMessages.Model;

public sealed record MidiMessage
{
    public MidiStatusByte Status { get; init; }
    public MidiDataByte Data1 { get; init; }
    public MidiDataByte Data2 { get; init; }

    public MidiMessage( MidiStatusByte statusByte, MidiDataByte? dataByte1 = null, MidiDataByte? dataByte2 = null )
    {
        Status = statusByte;
        Data1  = dataByte1 ?? MidiDataByte.None;
        Data2  = dataByte2 ?? MidiDataByte.None;
    }

    public static MidiMessage Create( int statusByte, int? dataByte1 = null, int? dataByte2 = null )
    {
        return new MidiMessage(
            new MidiStatusByte( statusByte ),
            dataByte1 != null ? new MidiDataByte( dataByte1.Value ) : null,
            dataByte2 != null ? new MidiDataByte( dataByte2.Value ) : null
        );
    }

    public override string ToString()
        => $"Midi Message: status={Status.Value:X2}, ({StatusType}), data1={Data1.Value:X2}, data2={Data2.Value:X2}";

    #region Status Byte Utilities
    // ReSharper disable MemberCanBePrivate.Global
    public bool IsChannelVoiceMessage
        => Status.Value is >= 0x80 and <= 0xEF && !IsChannelModeMessage;

    public bool IsChannelModeMessage
        => Status.Value is >= 0xB0 and <= 0xBF && Data1.Value is >= 0x78 and <= 0x7F;

    public bool IsSystemMessage
        => Status.Value is >= 0xF0 and <= 0xF7;

    public bool IsSystemRealTimeMessage
        => Status.Value is >= 0xF8 and <= 0xFF;
    // ReSharper restore MemberCanBePrivate.Global

    public MidiStatusType StatusType
    {
        // ReSharper disable once CyclomaticComplexity
        get
        {
            return Status.Value switch
            {
                // Channel Voice
                >= 0x80 and <= 0x8F => MidiStatusType.NoteOff,
                >= 0x90 and <= 0x9F => MidiStatusType.NoteOn,
                >= 0xA0 and <= 0xAF => MidiStatusType.PolyphonicKeyPressure,
                // Control Change (Data1:0-119) or Channel Mode Message (Data1:120-127)
                >= 0xB0 and <= 0xBF => MidiStatusType.ControlChangeOrChannelVoiceMessage,
                // Channel Voice
                >= 0xC0 and <= 0xCF => MidiStatusType.ProgramChange,
                >= 0xD0 and <= 0xDF => MidiStatusType.ChannelPressure,
                >= 0xE0 and <= 0xEF => MidiStatusType.PitchBendChange,
                // System Common Message
                0xF0 => MidiStatusType.SysExBegin,
                0xF1 => MidiStatusType.MidiTimeRecord,
                0xF2 => MidiStatusType.SongPosition,
                0xF3 => MidiStatusType.SongSelect,
                0xF4 => MidiStatusType.Undefined,
                0xF5 => MidiStatusType.Undefined,
                0xF6 => MidiStatusType.ChainRequest,
                0xF7 => MidiStatusType.SysExEnd,
                // System Realtime Message
                0xF8 => MidiStatusType.MidiClock,
                0xF9 => MidiStatusType.Undefined,
                0xFA => MidiStatusType.Start,
                0xFB => MidiStatusType.Continue,
                0xFC => MidiStatusType.Stop,
                0xFD => MidiStatusType.Undefined,
                0xFE => MidiStatusType.ActiveSensing,
                0xFF => MidiStatusType.Reset,
                _    => MidiStatusType.Undefined
            };
        }
    }
    #endregion ~Status Byte Utilitises

    #region Channel Message Mode Utilities
    public MidiChannelMessageModeType ChannelMessageModeType
    {
        get
        {
            if( StatusType != MidiStatusType.ControlChangeOrChannelVoiceMessage )
            {
                return MidiChannelMessageModeType.Undefined;
            }

            return Status.Value switch
            {
                >= 0xB0 and <= 0xBF when Data1.Value == 0x78 => MidiChannelMessageModeType.AllSoundOff,
                >= 0xB0 and <= 0xBF when Data1.Value == 0x79 => MidiChannelMessageModeType.ResetAllController,
                >= 0xB0 and <= 0xBF when Data1.Value == 0x7A => MidiChannelMessageModeType.LocalControl,
                >= 0xB0 and <= 0xBF when Data1.Value == 0x7B => MidiChannelMessageModeType.NotesOff,
                >= 0xB0 and <= 0xBF when Data1.Value == 0x7C => MidiChannelMessageModeType.OmniOff,
                >= 0xB0 and <= 0xBF when Data1.Value == 0x7D => MidiChannelMessageModeType.OmniOn,
                >= 0xB0 and <= 0xBF when Data1.Value == 0x7E => MidiChannelMessageModeType.MonoMode,
                >= 0xB0 and <= 0xBF when Data1.Value == 0x7F => MidiChannelMessageModeType.PolyMode,
                _                                            => MidiChannelMessageModeType.Undefined
            };
        }
    }
    #endregion ~Channel Message Mode Utilities

    #region Channel Utilities
    public bool TryGetChannel( out MidiChannel channel )
    {
        channel = null!;

        if( !IsChannelVoiceMessage || IsChannelModeMessage )
        {
            return false;
        }

        channel = new MidiChannel( Status.Value & 0x0F );

        return true;
    }
}
