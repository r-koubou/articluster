using System;
using System.Collections.Generic;
using System.Linq;

using ArtiCluster.Features.UniversalDefinitions.Infrastructures.Yaml.Model;
using ArtiCluster.Shared.Domain.Articulation.Model;
using ArtiCluster.Shared.Domain.MidiMessages.Model;

namespace ArtiCluster.Features.UniversalDefinitions.Infrastructures.Yaml;

internal static class DomainModelMapper
{
    public static UniversalDefinition Map( UniversalDefinitionModel source )
    {
        if( source == null )
        {
            throw new ArgumentNullException( nameof( source ) );
        }

        return new UniversalDefinition(
            id: source.Id,
            author: source.Author,
            manufacturerName: source.ManufacturerName,
            productName: source.ProductName,
            patchName: source.PatchName,
            description: source.Description,
            articulations: MapAssignments( source.Articulations ),
            extra: new Dictionary<string, string>( source.Extra )
        );
    }

    private static List<Articulation> MapAssignments( IEnumerable<ArticulationModel> source )
    {
        return source
              .Select( model => new Articulation(
                           name: model.Name,
                           midiMessages: model.MidiMessages.Select( x => new MidiMessage( x.Status, x.Data1, x.Data2 ) ).ToList(),
                           extra: new Dictionary<string, string>( model.Extra )
                       )
               )
              .ToList();
    }
}
