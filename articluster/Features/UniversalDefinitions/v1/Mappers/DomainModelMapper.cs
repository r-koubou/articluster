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
        if( source == null )
        {
            throw new ArgumentNullException( nameof( source ) );
        }

        if( source.FormatVersion != UniversalDefinitionModel.CurrentFormatVersion )
        {
            throw new UnsupportedFormatVersionException( source.FormatVersion );
        }

        return UniversalDefinition.Create(
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
              .Select( model => Articulation.Create(
                           name: model.Name,
                           midiMessages: model.MidiMessages.Select( x => MidiMessage.Create( x.Status, x.Data1, x.Data2, x.Channel ) ).ToList(),
                           extra: new Dictionary<string, string>( model.Extra )
                       )
               )
              .ToList();
    }
}
