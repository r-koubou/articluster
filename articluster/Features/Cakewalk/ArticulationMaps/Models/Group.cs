using System.Text.Json.Serialization;

namespace ArtiCluster.Features.Cakewalk.ArticulationMaps.Models;

public sealed class Group
{
    [JsonPropertyName( "id" )]
    public int Id { get; set; }

    [JsonPropertyName( "name" )]
    public string Name { get; set; } = string.Empty;
}
