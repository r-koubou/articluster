using System;
using System.Collections.Generic;

using ArtiCluster.Commons;
using ArtiCluster.Features.Cakewalk.ArticulationMaps.Models;
using ArtiCluster.Shared.Domain.MidiMessages.Model;
using ArtiCluster.Shared.Domain.MidiMessages.Model.Values;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

using Articulation = ArtiCluster.Features.Cakewalk.ArticulationMaps.Models.Articulation;

namespace ArtiCluster.Features.Cakewalk.ArticulationMaps.Mappers;

public sealed class CakewalkModelMapper
{
    public Result<CakewalkRootObject, Unit> Map( UniversalDefinitionProductSet source )
    {
        try
        {
            var result = new CakewalkRootObject();
            var articulationMaps = result.ArticulationMaps;
            var groups = ConvertArticulationGroup( source );
            var articulationMap = ConvertArticulationMap( source, groups );

            articulationMaps.Add( articulationMap );

            return Result<CakewalkRootObject, Unit>.Success( result );
        }
        catch( Exception e )
        {
            return Result<CakewalkRootObject, Unit>.Failure( Unit.Default, e );
        }
    }

    private static ArticulationMap ConvertArticulationMap(
        UniversalDefinitionProductSet source,
        IReadOnlyCollection<Group> groups )
    {
        var id = 1;
        var index = 0;

        var articulations = new List<Articulation>();

        foreach( var definition in source.Items )
        {
            foreach( var x in definition.Articulations )
            {
                var articulation = ConvertArticulation( definition, x, groups, id, index );
                articulations.Add( articulation );

                id++;
                index++;
            }
        }

        return new ArticulationMap
        {
            Name          = source.ProductName.Value,
            Groups        = new List<Group>( groups ),
            Articulations = articulations
        };
    }

    private static List<Group> ConvertArticulationGroup( UniversalDefinitionProductSet source )
    {
        var result = new List<Group>();
        var id = 1;

        foreach( var x in source.Items )
        {
            var g = new Group
            {
                Id = id,
                Name = x.PatchName.Value
            };

            result.Add( g );
            id++;
        }

        return result;
    }

    private static Articulation ConvertArticulation(
        UniversalDefinition definition,
        Shared.Domain.UniversalDefinitions.Model.Articulation articulation,
        IReadOnlyCollection<Group> groups,
        int id,
        int index )
    {
        if( !TryGetGroupId( definition, groups, out var groupId ) )
        {
            throw new InvalidOperationException( $"Group not found for articulation {articulation.Name.Value}" );
        }

        return new Articulation(
            id,
            articulation.Name.Value,
            index,
            groupId,
            "ff4da3b9",
            120, // 120: At Exported from Cakewalk
            ConvertArticulationEvents( articulation ),
            ConvertArticulationTransform( articulation )
        );
    }

    private static bool TryGetGroupId(
        UniversalDefinition source,
        IReadOnlyCollection<Group> groups,
        out int groupId )
    {
        groupId = -1;

        foreach( var x in groups )
        {
            if( x.Name == source.PatchName.Value )
            {
                groupId = x.Id;
                return true;
            }
        }

        return false;
    }

    private static List<Transform> ConvertArticulationTransform( Shared.Domain.UniversalDefinitions.Model.Articulation _ )
    {
        //TODO If support cakewalk dependent items
        return [ ];
    }

    private static IEnumerable<MidiEvent> ConvertArticulationEvents( Shared.Domain.UniversalDefinitions.Model.Articulation articulation )
    {
        var result = new List<MidiEvent>();

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach( var midiMessage in articulation.MidiMessages )
        {
            result.Add( CreateEvent( midiMessage ) );
        }

        return result;

        MidiEvent CreateEvent( MidiMessage x )
        {
            var midiChannel = x.Channel == MidiChannel.None
                ? 0
                : x.Channel.Value;

            var allowTransposeMidiCh = x.Channel == MidiChannel.None
                ? 1
                : 0;

            return new MidiEvent
            {
                Byte1 = x.Status.Value | midiChannel,
                Byte2 = x.Data1.Value,
                Byte3 = x.Data2.Value,
                AllowTransposeMidiCh = allowTransposeMidiCh
            };
        }
    }
}
