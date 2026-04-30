using System;

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
                0xF1 => MidiStatusType.MidiTimecode,
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
    #endregion ~Channel Utilities

    #region Data Byte Utilities
    public int RequireDataByteCount
    {
        get
        {
            return StatusType switch
            {
                MidiStatusType.NoteOff                            => 2,
                MidiStatusType.NoteOn                             => 2,
                MidiStatusType.PolyphonicKeyPressure              => 2,
                MidiStatusType.ControlChangeOrChannelVoiceMessage => 2,
                MidiStatusType.ProgramChange                      => 1,
                MidiStatusType.ChannelPressure                    => 1,
                MidiStatusType.PitchBendChange                    => 2,
                MidiStatusType.SysExBegin                         => -1, // Variable length
                MidiStatusType.MidiTimecode                       => 1,
                MidiStatusType.Start                              => 1,
                MidiStatusType.Continue                           => 1,
                MidiStatusType.Stop                               => 1,
                MidiStatusType.ActiveSensing                      => 1,
                MidiStatusType.Reset                              => 1,
                _                                                 => -1
            };
        }
    }

    public bool TryGetRequireDataByteCount( out int count )
    {
        try
        {
            count = RequireDataByteCount;
            return true;
        }
        catch
        {
            count = -1;
            return false;
        }
    }

    /// <summary>
    /// Attempt to retrieve the value of the data byte based on the value of the MIDI status byte
    /// </summary>
    /// <param name="count">Store data byte count (if -1, couldn't get )</param>
    /// <param name="data1">Store data byte 1 value (if -1, couldn't get )</param>
    /// <param name="data2">Store data byte 2 value (if -1, couldn't get )</param>
    /// <return>
    /// <list type="bullet">
    ///   <item>If the MIDI status cannot be determined, return false</item>
    ///   <item>Returns false if the required data byte <see cref="Data1"/> or <see cref="Data2"/> is <see cref="MidiDataByte.None"/></item>
    /// </list>
    /// </return>
    public bool TryGetDataByte1( out int count, out int data1, out int data2 )
    {
        count = data1 = data2 = -1;

        if( !TryGetRequireDataByteCount( out count ) )
        {
            return false;
        }

        switch( count )
        {
            case 1 when Data1 == MidiDataByte.None:
            case 2 when ( Data1 == MidiDataByte.None || Data2 == MidiDataByte.None ):
                return false;
            case 1:
                data1 = Data1.Value;
                return true;
            case 2:
                data1 = Data2.Value;
                data2 = Data2.Value;
                return true;
            default:
                return false;
        }
    }
    #endregion ~Data Byte Utilities
}
