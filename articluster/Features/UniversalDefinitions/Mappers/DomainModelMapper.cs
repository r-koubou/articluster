using System;
using System.Collections.Generic;
using System.Linq;

using ArtiCluster.Features.UniversalDefinitions.v1.Models;
using ArtiCluster.Shared.Domain.MidiMessages.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Features.UniversalDefinitions.v1.Mappers;

public static class DomainModelMapper
{
    public static UniversalDefinition Map( UniversalDefinitionModel source )
    {
        ArgumentNullException.ThrowIfNull( source );

        return UniversalDefinition.Create(
            id: source.Id,
            author: source.Author,
            manufacturerName: source.ManufacturerName,
            productName: source.ProductName,
            patchName: source.PatchName,
            description: source.Description,
            articulationGroups: MapArticulationGroups( source.ArticulationGroups ),
            extra: new Dictionary<string, string>( source.Extra )
        );
    }

    private static List<ArticulationGroup> MapArticulationGroups( IEnumerable<ArticulationGroupModel> source )
    {
        // @formatter:off
        return source
              .Select( model => ArticulationGroup.Create(
                           name: model.Name,
                           articulations: model.Articulations.Select( x => Articulation.Create(
                                name: x.Name,
                                midiMessages: x.MidiMessages.Select( m => MidiMessage.Create( m.Status, m.Data1, m.Data2, m.Channel ) ).ToList(),
                                extra: new Dictionary<string, string>( x.Extra )
                            )).ToList(),
                           extra: new Dictionary<string, string>( model.Extra )
                       )
               )
              .ToList();
        // @formatter:on
    }
}
