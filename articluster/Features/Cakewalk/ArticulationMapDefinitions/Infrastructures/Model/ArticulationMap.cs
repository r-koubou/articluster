using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.Infrastructures.Model;

public class ArticulationMap
{
    [JsonPropertyName( "name" )]
    [JsonRequired]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName( "groups" )]
    public List<Group> Groups { get; set; } = [ ];

    [JsonPropertyName( "articulations" )]
    public IList<Articulation> Articulations { get; set; } = [ ];
}
