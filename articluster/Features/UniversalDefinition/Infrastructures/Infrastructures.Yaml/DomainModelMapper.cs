using System;
using System.Collections.Generic;
using System.Linq;

using ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model;
using ArtiCluster.Shared.Domain.Articulation.Model;
using ArtiCluster.Shared.Domain.MidiMessages.Model;

namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml;

internal static class DomainModelMapper
{
    public static Articulation Map( RootModel source )
    {
        if( source == null )
        {
            throw new ArgumentNullException( nameof( source ) );
        }

        return new Articulation(
            id: source.Id,
            author: source.Author,
            manufacturerName: source.ManufacturerName,
            productName: source.ProductName,
            patchName: source.PatchName,
            description: source.Description,
            assignments: MapAssignments( source.Assignments ),
            extra: new Dictionary<string, string>( source.Extra )
        );
    }

    private static List<Assignment> MapAssignments( IEnumerable<AssignmentModel> source )
    {
        return source
              .Select( model => new Assignment(
                           name: model.Name,
                           midiMessages: model.MidiMessages.Select( x => new MidiMessage( x.Status, x.Data1, x.Data2 ) ).ToList(),
                           extra: new Dictionary<string, string>( model.Extra )
                       )
               )
              .ToList();
    }
}
