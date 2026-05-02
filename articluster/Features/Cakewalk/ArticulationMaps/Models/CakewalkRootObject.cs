using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ArtiCluster.Features.Cakewalk.ArticulationMaps.Models;

public class CakewalkRootObject
{
    [JsonPropertyName( "ArticulationMaps" )]
    public IList<ArticulationMap> ArticulationMaps { get; set; } = new List<ArticulationMap>();
}
