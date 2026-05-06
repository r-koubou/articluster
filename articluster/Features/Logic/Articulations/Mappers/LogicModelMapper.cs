using System.Collections.Generic;
using System.Linq;

using ArtiCluster.Shared.Domain.MidiMessages.Model;
using ArtiCluster.Shared.Domain.MidiMessages.Model.Values;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

using Claunia.PropertyList;

namespace ArtiCluster.Features.Logic.Articulations.Mappers;

public sealed class LogicModelMapper
{
    public NSDictionary Map( UniversalDefinition source )
    {
        var result = new NSDictionary();
        var id = 1001;
        var articulationId = 1;

        #region Articulations
        {
            var articulations = new NSArray();

            foreach( var articulation in source.Articulations )
            {
                articulations.Add( ConvertArticulation( articulation, id, articulationId ) );
                id++;
                articulationId++;
            }

            result.Add( "Articulations", articulations );
        }
        #endregion

        #region Switches
        {
            result.Add( "Switches", new NSArray() );
        }
        #endregion

        #region MultipleOutputsActive
        {
            var multipleOutputsActive = source.Articulations.Any( x => x.MidiMessages.Count >= 2 );
            result.Add( "MultipleOutputsActive", multipleOutputsActive );
        }
        #endregion

        result.Add( "Name", $"{source.PatchName.Value}.plist" );
        result.Add( "OctaveOffset", 0 );

        return result;
    }

    private static NSDictionary ConvertArticulation( Articulation articulation, int id, int articulationId )
    {
        var outputArray = new NSArray();

        ConvertChannelVoiceMessageList( articulation.MidiMessages, outputArray );

        var result = new NSDictionary
        {
            { "ArticulationID", articulationId },
            { "ID", id },
            { "Name", articulation.Name.Value },
            { "Output", outputArray }
        };

        return result;
    }

    private static void ConvertChannelVoiceMessageList( IEnumerable<MidiMessage> messages, NSArray dest )
    {
        foreach( var message in messages )
        {
            var midiMessageDictionary = new NSDictionary();

            int? data1 = message.Data1 == MidiDataByte.None ? null : message.Data1.Value;
            int? data2 = message.Data2 == MidiDataByte.None ? null : message.Data2.Value;

            if( !ConvertMessageStatus( message, midiMessageDictionary ) )
            {
                continue;
            }

            if( message.Channel != MidiChannel.None )
            {
                midiMessageDictionary.Add( "MidiChannel", message.Channel.Value );
            }

            if( data1 != null )
            {
                midiMessageDictionary.Add( "MB1", data1 );
            }

            if( data2 != null )
            {
                midiMessageDictionary.Add( "ValueLow", data2 );
            }

            dest.Add( midiMessageDictionary );
        }
    }

    private static bool ConvertMessageStatus( MidiMessage message, NSDictionary midiMessageDictionary )
    {
        switch( message.StatusType )
        {
            case MidiStatusType.NoteOn:
                midiMessageDictionary.Add( "Status", "Note On" );
                break;

            case MidiStatusType.NoteOff:
                midiMessageDictionary.Add( "Status", "Note Off" );
                break;

            case MidiStatusType.ChannelPressure:
                midiMessageDictionary.Add( "Status", "Aftertouch" );
                break;

            case MidiStatusType.ControlChangeOrChannelVoiceMessage when message.IsControlChangeMessage:
                midiMessageDictionary.Add( "Status", "Controller" );
                break;

            case MidiStatusType.ProgramChange:
                midiMessageDictionary.Add( "Status", "Program" );
                break;

            case MidiStatusType.PolyphonicKeyPressure:
                midiMessageDictionary.Add( "Status", "Poly Aftertouch" );
                break;

            case MidiStatusType.PitchBendChange:
                midiMessageDictionary.Add( "Status", "Pitch Bend" );
                break;
            default:
                return false;
        }

        return true;
    }
}
