using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.Logic.ArticulationSetDefinitions.Gateways;
using ArtiCluster.Shared.Domain.MidiMessages.Model;
using ArtiCluster.Shared.Domain.MidiMessages.Model.Values;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

using Claunia.PropertyList;

namespace ArtiCluster.Features.Logic.ArticulationSetDefinitions.Infrastructures;

public class LogicExporter : IDefinitionExporter
{
    public async Task<Result<Unit, ExportReason>> ExportAsync( ITextContentWriter writer, UniversalDefinition source, CancellationToken cancellationToken = default )
    {
        try
        {
            var root = ConvertRootNsDictionary( source );
            using var memoryStream = new MemoryStream();

            PropertyListParser.SaveAsXml( root, memoryStream );
            memoryStream.Position = 0;

            using var textReader = new StreamReader( memoryStream );
            await writer.WriteAsync( await textReader.ReadToEndAsync( cancellationToken ), cancellationToken );

            return Result<Unit, ExportReason>.Success( Unit.Default );
        }
        catch( IOException e )
        {
            return Result<Unit, ExportReason>.Failure( ExportReason.IoError, e );
        }
        catch( Exception e )
        {
            return Result<Unit, ExportReason>.Failure( ExportReason.OtherError, e );
        }

    }

    private static NSDictionary ConvertRootNsDictionary( UniversalDefinition source )
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

            var data1 = message.Data1.Value;
            var data2 = message.Data2.Value;

            midiMessageDictionary.Add( "MB1", data1 );

            // ToDo 他のStatusTypeも必要に応じて追加する(要Logicで書き出してチェックが必要)
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

                case MidiStatusType.ControlChange:
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
                    continue;
            }

            midiMessageDictionary.Add( "ValueLow", data2 );

            dest.Add( midiMessageDictionary );
        }
    }
}
