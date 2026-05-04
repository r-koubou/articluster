using System.Linq;

using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Applications.Cli.Editor.Model;

public static class UniversalDefinitionMapper
{
    public static UniversalDefinitionModel FromDomain( UniversalDefinition definition )
    {
        return new UniversalDefinitionModel
        {
            Id               = definition.Id,
            Author           = definition.Author.Value,
            ManufacturerName = definition.ManufacturerName.Value,
            ProductName      = definition.ProductName.Value,
            PatchName        = definition.PatchName.Value,
            Articulations = definition.Articulations.Select( x => new ArticulationModel
                {
                    Name = x.Name.Value,
                    MidiMessages = x.MidiMessages.Select( m => new MidiMessageModel
                        {
                            Status = m.Status.Value,
                            Data1  = m.Data1.Value,
                            Data2  = m.Data2.Value
                        }
                    ).ToList()
                }
            ).ToList()
        };
    }
}
