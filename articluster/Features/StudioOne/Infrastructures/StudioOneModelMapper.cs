using System.Collections.Generic;
using System.Linq;
using System.Text;

using ArtiCluster.Commons;
using ArtiCluster.Features.StudioOne.Gateways;
using ArtiCluster.Features.StudioOne.Infrastructures.Model;
using ArtiCluster.Shared.Domain.MidiMessages.Model.Values;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Features.StudioOne.Infrastructures;

public sealed class StudioOneModelMapper : IStudioOneModelMapper
{
    public Result<StudioOneRootElement, ExportReason> Map( UniversalDefinitionProductSet source )
    {
        if( source.IsEmpty )
        {
            return Result<StudioOneRootElement, ExportReason>.Success(
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

        return Result<StudioOneRootElement, ExportReason>.Success( rootElement );
    }

    private static Result<StudioOneRootElement, ExportReason> MapWithFolders( UniversalDefinitionProductSet source, int assignId )
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

            var folder = new AttributeElement
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

        return Result<StudioOneRootElement, ExportReason>.Success( rootElement );
    }

    private static List<AttributeElement> MapElementAttributes( IReadOnlyCollection<UniversalDefinition> sources, ref int assignId )
    {
        var result = new List<AttributeElement>();

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

    private static AttributeElement MapElementAttribute( Articulation articulation, int assignId )
    {
        var name = articulation.Name.Value;
        var pitch = AttributeElement.NoPitch;
        var activation = MapActivation( articulation );

        var midiNoteOns = articulation.MidiMessages.Where( message => message.StatusType == MidiStatusType.NoteOn ).ToList();

        if( midiNoteOns.Count != 0 )
        {
            pitch = midiNoteOns.Single().Data1.Value;
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

        return new AttributeElement( name, assignId, color, pitch, momentary, activation );
    }

    private static string MapActivation( Articulation articulation )
    {
        var activations = new List<string>();
        var sb = new StringBuilder( 128 );

        activations.AddRange( MapActivationNote( articulation ) );
        activations.AddRange( MapActivationControlChange( articulation ) );
        activations.AddRange( MapActivationProgramChange( articulation ) );

        activations = activations.Distinct().ToList();

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

    private static List<string> MapActivationNote( Articulation articulation )
    {
        var result = new List<string>();

        var midiNoteOns = articulation.MidiMessages.Where( message => message.StatusType == MidiStatusType.NoteOn ).ToList();
        var midiNoteOffs = articulation.MidiMessages.Where( message => message.StatusType == MidiStatusType.NoteOff ).ToList();

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach( var x in midiNoteOns )
        {
            var byte1 = x.Data1.Value;
            var byte2 = x.Data2.Value;
            result.Add( $"note{byte1}.{byte2}" );
        }

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach( var x in midiNoteOffs )
        {
            var byte1 = x.Data1.Value;
            var byte2 = x.Data2.Value;
            result.Add( $"off{byte1}.{byte2}" );
        }

        return result;
    }

    private static List<string> MapActivationControlChange( Articulation articulation )
    {
        var result = new List<string>();
        var controlChanges = articulation.MidiMessages.Where( message => message.StatusType == MidiStatusType.ControlChange ).ToList();

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach( var x in controlChanges )
        {
            var ccNo = x.Status.Value;
            var byte1 = x.Data1.Value;
            var byte2 = x.Data2.Value;

            result.Add(
                ccNo is 0 or 32
                    ? $"bc{byte1}.{byte2}"
                    : $"cc{byte1}.{byte2}"
            );
        }

        return result;
    }

    private static List<string> MapActivationProgramChange( Articulation articulation )
    {
        var result = new List<string>();
        var programChanges = articulation.MidiMessages.Where( message => message.StatusType == MidiStatusType.ProgramChange ).ToList();

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach( var x in programChanges )
        {
            var byte1 = x.Data1.Value;
            result.Add( $"pc{byte1}" );
        }

        return result;
    }
}
