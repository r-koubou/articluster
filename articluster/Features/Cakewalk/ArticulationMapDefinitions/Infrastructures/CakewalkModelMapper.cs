using System.Collections.Generic;

using ArtiCluster.Commons;
using ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.Gateways;
using ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.Infrastructures.Model;
using ArtiCluster.Shared.Domain.MidiMessages.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

using Articulation = ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.Infrastructures.Model.Articulation;

namespace ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.Infrastructures;

public class CakewalkModelMapper : IModelMapper<CakewalkRootObject>
{
    public Result<CakewalkRootObject, ExportReason> Map( UniversalDefinitionProductSet source )
    {
        var result = new CakewalkRootObject();
        var patches = result.ArticulationMaps;

        foreach( var definition in source.Items )
        {
            var articulationMap = ConvertArticulationMap( definition );
            patches.Add( articulationMap );
        }

        return Result<CakewalkRootObject, ExportReason>.Success( result );
    }

    private static ArticulationMap ConvertArticulationMap( UniversalDefinition source )
    {
        var id = 1;
        var index = 0;
        var groupId = 1;

        var articulations = new List<Articulation>();

        foreach( var x in source.Articulations )
        {
            var a = ConvertArticulation( x, id, index, groupId );
            articulations.Add( a );

            id++;
            index++;
        }

        return new ArticulationMap
        {
            Name = source.ProductName.Value,
            Groups =
            [
                new Group( 1, source.PatchName.Value )
            ],
            Articulations = articulations
        };
    }

    private static Articulation ConvertArticulation( Shared.Domain.UniversalDefinitions.Model.Articulation articulation, int id, int index, int groupId )
    {
        return new Articulation(
            id,
            articulation.Name.Value,
            index,
            groupId,
            "ffff0000",
            0,
            ConvertArticulationEvents( articulation ),
            ConvertArticulationTransform( articulation )
        );
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
            return new MidiEvent
            {
                Byte1 = x.Status.Value,
                Byte2 = x.Data1.Value,
                Byte3 = x.Data2.Value,
            };
        }
    }
}
