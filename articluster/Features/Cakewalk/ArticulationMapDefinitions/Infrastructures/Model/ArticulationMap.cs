using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.Infrastructures.Model;

public class ArticulationMap
{
    [JsonPropertyName( "name)" )]
    [JsonRequired]
    public string Name { get; }

    [JsonPropertyName( "groups" )]
    public IList<Group> Groups { get; }

    [JsonPropertyName( "articulations" )]
    public IList<Articulation> Articulations { get; }

    public ArticulationMap(
        string name,
        IList<Group> groups,
        IList<Articulation> articulations )
    {
        Name          = name;
        Groups        = groups;
        Articulations = articulations;
    }
}
