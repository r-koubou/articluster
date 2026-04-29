using System.Text.Json.Serialization;

namespace ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.Infrastructures.Model;

public class Group
{
    [JsonPropertyName( "id" )]
    public int Id { get; }

    [JsonPropertyName( "name" )]
    public string Name { get; }

    public Group( int id, string name )
    {
        Id   = id;
        Name = name;
    }
}
