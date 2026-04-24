using System.Collections.Generic;
using System.Linq;

using ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model;
using ArtiCluster.Shared.Domain.Articulation.Model;
using ArtiCluster.Shared.Domain.MidiMessages.Model.Values;

namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml;

internal static class YamlModelMapper
{
    public static RootModel Map( Articulation source )
    {
        return new RootModel
        {
            Id               = source.Id,
            Author           = source.Author.Value,
            ManufacturerName = source.ManufacturerName.Value,
            ProductName      = source.ProductName.Value,
            PatchName        = source.PatchName.Value,
            Description      = source.Description.Value,
            Assignments      = MapAssignment( source.Assignments ),
            Extra            = new Dictionary<string, string>( source.Extra )
        };
    }

    private static List<AssignmentModel> MapAssignment( IEnumerable<Assignment> source )
    {
        return source
              .Select( assignment => new AssignmentModel
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
