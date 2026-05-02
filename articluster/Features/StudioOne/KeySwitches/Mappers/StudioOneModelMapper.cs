using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArtiCluster.Commons;
using ArtiCluster.Features.StudioOne.KeySwitches.Contracts;
using ArtiCluster.Features.StudioOne.KeySwitches.Models;
using ArtiCluster.Shared.Domain.MidiMessages.Model.Values;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Features.StudioOne.KeySwitches.Mappers;

public sealed class StudioOneModelMapper
{
    public Result<StudioOneRootElement, ExportFailureReason> Map( UniversalDefinitionProductSet source )
    {
        if( source.IsEmpty )
        {
            return Result<StudioOneRootElement, ExportFailureReason>.Success(
                new StudioOneRootElement
                {
                    Name = $"{source.ProductName.Value}"
                }
            );
        }

        var assignId = 0;

        // Create with folder element if several patches exist
        if( source.Count >= 2 )
        {
            return MapWithFolders( source, assignId );
        }

#if false
    <?xml version = "1.0" encoding = "utf-8"?>
       <Music.KeySwitchList name = "Super Synth">
         <Attributes name = "Sustain" id = "0" pitch = "40" momentary = "0" activation = "note40.100|off40.110|cc1.127|pc49" />
         <Attributes name = "Sustain" id = "1" pitch = "40" momentary = "0" activation = "note40.100|off40.110|cc1.127|pc49" />
       </Music.KeySwitchList>
#endif
        var definition = source.Items.Single();
        var rootElement = new StudioOneRootElement
        {
            Name = $"{source.ProductName.Value} {definition.PatchName.Value}"
        };

        var attributeElements = MapElementAttributes( source.Items, ref assignId );
        rootElement.AttributeElements.AddRange( attributeElements );

        return Result<StudioOneRootElement, ExportFailureReason>.Success( rootElement );
    }

    private static Result<StudioOneRootElement, ExportFailureReason> MapWithFolders( UniversalDefinitionProductSet source, int assignId )
    {
#if false
    <?xml version = "1.0" encoding = "utf-8"?>
       <Music.KeySwitchList name = "Super Synth">
         <Attributes folder = "1" name = "Epic Lead">
           <Attributes name = "Sustain" id = "0" pitch = "40" momentary = "0" activation = "note40.100|off40.110|cc1.127|pc49" />
         </Attributes>
         <Attributes folder = "1" name = "E.Bass">
           <Attributes name = "Sustain" id = "1" pitch = "40" momentary = "0" activation = "note40.100|off40.110|cc1.127|pc49" />
         </Attributes>
       </Music.KeySwitchList>
#endif
        var rootElement = new StudioOneRootElement
        {
            Name = $"{source.ProductName.Value}"
        };

        foreach( var definition in source.Items )
        {
            if( definition.Articulations.Count == 0 )
            {
                continue;
            }

            var folder = new ElementAttribute
            {
                Folder = "1",
                Name   = definition.PatchName.Value
            };

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach( var articulation in definition.Articulations )
            {
                var attributeElement = MapElementAttribute( articulation, assignId );
                folder.Children.Add( attributeElement );
                assignId++;
            }

            rootElement.AttributeElements.Add( folder );
        }

        return Result<StudioOneRootElement, ExportFailureReason>.Success( rootElement );
    }

    private static List<ElementAttribute> MapElementAttributes( IReadOnlyCollection<UniversalDefinition> sources, ref int assignId )
    {
        var result = new List<ElementAttribute>();

        foreach( var definition in sources )
        {
            foreach( var articulation in definition.Articulations )
            {
                var attr = MapElementAttribute( articulation, assignId );
                assignId++;

                result.Add( attr );
            }
        }

        return result;
    }

    private static ElementAttribute MapElementAttribute( Articulation articulation, int assignId )
    {
        var name = articulation.Name.Value;
        var pitch = ElementAttribute.NoPitch;
        var activation = MapActivation( articulation );

        var midiNoteOns = articulation.MidiMessages.Where( message => message.StatusType == MidiStatusType.NoteOn ).ToList();

        if( midiNoteOns.Count > 0 )
        {
            pitch = midiNoteOns.First().Data1.Value;
        }

        string? color = null;

        if( articulation.Extra.TryGetValue( ExtraDataKeys.Color, out var value ) )
        {
            color = value;
        }

        var momentary = 0;

        if( articulation.Extra.TryGetValue( ExtraDataKeys.Momentary, out var momentaryValue ) )
        {
            momentary = momentaryValue == "0" ? 0 : 1;
        }

        return new ElementAttribute( name, assignId, color, pitch, momentary, activation );
    }

    private static string MapActivation( Articulation articulation )
    {
        var activations = MapActivationSequence( articulation );
        var sb = new StringBuilder( 128 );
        var count = activations.Count;

        for( var i = 0; i < count; i++ )
        {
            var x = activations[ i ];
            sb.Append( x );

            if( i < count - 1 )
            {
                sb.Append( '|' );
            }
        }

        return sb.ToString();
    }

    private static List<string> MapActivationSequence( Articulation articulation )
    {
        var result = new List<string>();

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach( var message in articulation.MidiMessages )
        {
            var type = message.StatusType;
            var data1 = message.Data1.Value;
            var data2 = message.Data2.Value;

            result.Add( type switch
                {
                    MidiStatusType.NoteOn  => $"note{data1}.{data2}",
                    MidiStatusType.NoteOff => $"off{data1}.{data2}",
                    MidiStatusType.ControlChangeOrChannelVoiceMessage when message.IsControlChangeMessage => data1 switch
                    {
                        0 or 32 => $"bc{data1}.{data2}",
                        _       => $"cc{data1}.{data2}"
                    },
                    MidiStatusType.ProgramChange => $"pc{data1}",
                    _                            => ""
                }
            );
        }

        return result;
    }
}
