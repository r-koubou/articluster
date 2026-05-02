using System.Collections.Generic;
using System.Linq;

using ArtiCluster.Features.UniversalDefinitionsNew.Models;
using ArtiCluster.Shared.Domain.MidiMessages.Model.Values;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Features.UniversalDefinitionsNew.Mappers;

public static class YamlModelMapper
{
    public static UniversalDefinitionModel Map( UniversalDefinition source )
    {
        return new UniversalDefinitionModel
        {
            Id               = source.Id,
            Author           = source.Author.Value,
            ManufacturerName = source.ManufacturerName.Value,
            ProductName      = source.ProductName.Value,
            PatchName        = source.PatchName.Value,
            Description      = source.Description.Value,
            Articulations    = MapAssignment( source.Articulations ),
            Extra            = new Dictionary<string, string>( source.Extra )
        };
    }

    private static List<ArticulationModel> MapAssignment( IEnumerable<Articulation> source )
    {
        return source
              .Select( assignment => new ArticulationModel
                   {
                       Name = assignment.Name.Value,
                       MidiMessages = assignment.MidiMessages.Select( x => new MidiMessageModel
                           {
                               Status = x.Status.Value,
                               Data1  = x.Data1 == MidiDataByte.None ? null : x.Data1.Value,
                               Data2  = x.Data2 == MidiDataByte.None ? null : x.Data2.Value,
                           }
                       ).ToList(),
                       Extra = new Dictionary<string, string>( assignment.Extra )
                   }
               )
              .ToList();
    }
}
