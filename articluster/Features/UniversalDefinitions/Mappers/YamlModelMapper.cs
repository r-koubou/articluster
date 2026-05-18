using System.Collections.Generic;
using System.Linq;

using ArtiCluster.Features.UniversalDefinitions.Models;
using ArtiCluster.Shared.Domain.MidiMessages.Model.Values;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Features.UniversalDefinitions.Mappers;

public static class YamlModelMapper
{
    public static UniversalDefinitionModel Map( UniversalDefinition source )
    {
        return new UniversalDefinitionModel
        {
            Id                 = source.Id,
            Author             = source.Author.Value,
            ManufacturerName   = source.ManufacturerName.Value,
            ProductName        = source.ProductName.Value,
            PatchName          = source.PatchName.Value,
            Description        = source.Description.Value,
            ArticulationGroups = MapArticulationGroups( source.ArticulationGroups ),
            Extra              = new Dictionary<string, string>( source.Extra )
        };
    }

    private static List<ArticulationGroupModel> MapArticulationGroups( IEnumerable<ArticulationGroup> source )
    {
        // @formatter:off
        return source
           .Select( assignment => new ArticulationGroupModel
                {
                    Name = assignment.Name.Value,
                    Articulations = assignment.Articulations.Select( x => new ArticulationModel
                        {
                            Name = x.Name.Value,
                            MidiMessages = x.MidiMessages.Select(
                                m => new MidiMessageModel(
                                    m.Status.Value,
                                    m.Data1 == MidiDataByte.None ? null : m.Data1.Value,
                                    m.Data2 == MidiDataByte.None ? null : m.Data2.Value,
                                    m.Channel == MidiChannel.None ? null : m.Channel.Value )
                            ).ToList(),
                            Extra = new Dictionary<string, string>( x.Extra )
                        }
                    ).ToList(),
                    Extra = new Dictionary<string, string>( assignment.Extra )
                }
            )
           .ToList();
        // @formatter:on
    }
}
