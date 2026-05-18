using System;
using System.Collections.Generic;

using ArtiCluster.Commons;
using ArtiCluster.Features.Cubase.ExpressionMaps.Contracts;
using ArtiCluster.Features.Cubase.ExpressionMaps.Models;
using ArtiCluster.Features.Cubase.ExpressionMaps.Models.XmlClasses;
using ArtiCluster.Shared.Domain.MidiMessages.Model.Values;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Features.Cubase.ExpressionMaps.Mappers;

public sealed class CubaseModelMapper
{
    public Result<RootElement, ExportFailureReason> Map( SeparatedArticulationGroupSet source )
    {
        try
        {
            var slotTable = CollectSlotTable( source );

            var listOfUSlotVisuals = ConvertUSlotVisualsList( source );
            var listOfPSoundSlot = ConvertPSoundSlotList( slotTable );

            var rootElement = ConvertRootElement(
                source,
                listOfPSoundSlot,
                listOfUSlotVisuals
            );

            return Result<RootElement, ExportFailureReason>.Success( rootElement );
        }
        catch( Exception e )
        {
            return Result<RootElement, ExportFailureReason>.Failure( ExportFailureReason.SerializationError, e );
        }
    }

    #region Convert RootElement
    private static RootElement ConvertRootElement( SeparatedArticulationGroupSet source, ListElement listOfPSoundSlot, ListElement listOfUSlotVisuals )
    {
        // Construction of InstrumentMap element
        var slots = InstrumentMap.Slots( listOfPSoundSlot );
        var slotVisuals = InstrumentMap.SlotVisuals( listOfUSlotVisuals );

        var instrumentName = $"{source.ProductName.Value} {source.PatchName.Value}";
        var rootElement = InstrumentMap.New( instrumentName );
        rootElement.Member.Add( slotVisuals );
        rootElement.Member.Add( slots );
        rootElement.StringElement.Value = instrumentName;

        return rootElement;
    }
    #endregion Convert RootElement

    #region Convert To USlotVisual List
    private static ListElement ConvertUSlotVisualsList( SeparatedArticulationGroupSet source )
    {
        var listOfUSlotVisuals = new ListElement();

        foreach( var articulation in source.Items )
        {
            var type = ConvertArticulationType( articulation.Extra.GetValueOrDefault( ExtraKeys.ArticulationType, string.Empty ) );
            var group = ConvertArticulationGroup( articulation.Extra.GetValueOrDefault( ExtraKeys.GroupIndex, string.Empty ) );

            var slotVisual = USlotVisuals.New( articulation, 0, type, group );
            listOfUSlotVisuals.Obj.Add( slotVisual );
        }

        return listOfUSlotVisuals;
    }
    #endregion ~Convert To USlotVisual List

    #region Convert To PSoundSlot List
    private static ListElement ConvertPSoundSlotList( IReadOnlyDictionary<string, ICollection<Articulation>> slotTable )
    {
        var listOfPSoundSlot = new ListElement();

        foreach( var pair in slotTable )
        {
            var slotName = pair.Key;

            // PSoundSlot
            // PSoundSlot.name
            var pSoundSlot = ConvertPSoundSlot( slotName );

            // PSoundSlot -> PSlotMidiAction -> POutputEvent
            var listOfPOutputEvent = ConvertPOutputEventList( pair.Value );

            // PSoundSlot -> PSlotMidiAction
            pSoundSlot.Obj.Add( PSlotMidiAction.New( listOfPOutputEvent ) );

            // PSoundSlot.sv
            var slotVisualList = ConvertSlotVisualList( pair.Value );
            pSoundSlot.Member.Add( PSoundSlot.Sv( slotVisualList ) );

            // PSoundSlot.color
            // TODO アーティキュレーション変数にアクセスできないため、 ExtraData の Color を参照できず
            //pSoundSlot.Int.Add( new IntElement( "color", ConvertColorIndex( ....[ExtraKeys.Color] ) ) );
            pSoundSlot.Int.Add( new IntElement( "color", ExtraKeys.DefaultColorIndex ) );

            // Aggregate
            listOfPSoundSlot.Obj.Add( pSoundSlot );
        }

        return listOfPSoundSlot;
    }

    private static ObjectElement ConvertPSoundSlot( string slotName )
    {
        // PSoundSlot
        // PSoundSlot.name
        var pSoundSlot = PSoundSlot.New( slotName );

        // PSoundSlot.PSlotThruTrigger
        pSoundSlot.Obj.Add( PSlotThruTrigger.New() );

        return pSoundSlot;
    }

    private static ListElement ConvertPOutputEventList( IEnumerable<Articulation> articulations )
    {
        // PSoundSlot -> PSlotMidiAction -> POutputEvent
        var listOfPOutputEvent = new ListElement();

        foreach( var articulation in articulations )
        {
            ConvertOutputMappings( articulation, listOfPOutputEvent );
        }

        return listOfPOutputEvent;
    }

    private static List<ObjectElement> ConvertSlotVisualList( IEnumerable<Articulation> articulations )
    {
        // PSoundSlot.sv
        var slotVisualList = new List<ObjectElement>();

        foreach( var articulation in articulations )
        {
            var type = ConvertArticulationType( articulation.Extra.GetValueOrDefault( ExtraKeys.ArticulationType, string.Empty ) );
            var group = ConvertArticulationGroup( articulation.Extra.GetValueOrDefault( ExtraKeys.GroupIndex, string.Empty ) );

            slotVisualList.Add( USlotVisuals.New( articulation, 0, type, group ) );
        }

        return slotVisualList;
    }

    private static IReadOnlyDictionary<string, ICollection<Articulation>> CollectSlotTable( SeparatedArticulationGroupSet source )
    {
        static void AddArticulation( IDictionary<string, ICollection<Articulation>> dictionary, string key, Articulation articulation )
        {
            if( !dictionary.ContainsKey( key ) )
            {
                dictionary[ key ] = new List<Articulation>();
            }

            dictionary[ key ].Add( articulation );
        }

        var result = new Dictionary<string, ICollection<Articulation>>();

        foreach( var articulation in source.Items )
        {
            // use an articulation name as slot name if user does not define a slot name
            if( !articulation.Extra.TryGetValue( ExtraKeys.SlotName, out var extraValue ) )
            {
                AddArticulation( result, articulation.Name.Value, articulation );

                continue;
            }

            // User can assign articulation to any slot by comma ( , ) separated
            var slotNames = extraValue.Split( ',' );

            foreach( var slotName in slotNames )
            {
                var key = slotName.Trim();

                if( key.Length == 0 )
                {
                    continue;
                }

                AddArticulation( result, key, articulation );
            }
        }

        return result;
    }
    #endregion ~Convert To PSoundSlot List

    #region Sub Routines
    private static int ConvertArticulationType( string value )
    {
        return value switch
        {
            "Attribute" => 0,
            // ReSharper disable once RedundantSwitchExpressionArms
            "Direction" => 1,
            _           => 1
        };
    }

    private static int ConvertArticulationGroup( string value )
    {
        return int.TryParse( value, out var result ) ? result : 0;
    }

    // ReSharper disable once UnusedMember.Local
    private static int ConvertColorIndex( string value )
    {
        return int.TryParse( value, out var result ) ? result : 1;
    }

    private static void ConvertOutputMappings( Articulation articulation, ListElement listOfPOutputEvent )
    {
        foreach( var message in articulation.MidiMessages )
        {
            var channel = message.Channel == MidiChannel.None
                ? 0
                : message.Channel.Value;

            var status = message.Status.Value;
            int? data1 = message.Data1 == MidiDataByte.None ? null : message.Data1.Value;
            int? data2 = message.Data2 == MidiDataByte.None ? null : message.Data2.Value;

            listOfPOutputEvent.Obj.Add( POutputEvent.New( status | channel, data1, data2 ) );
        }
    }
    #endregion ~Sub Routines
}
