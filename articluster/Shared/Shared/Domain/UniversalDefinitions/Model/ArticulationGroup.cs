using System.Collections.Generic;

using ArtiCluster.Shared.Domain.UniversalDefinitions.Model.Values;

namespace ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

public sealed record ArticulationGroup
{
    public ArticulationGroupName Name { get; init; }
    public IReadOnlyCollection<Articulation> Articulations { get; init; }
    public IReadOnlyDictionary<string, string> Extra { get; init; }

    // ReSharper disable once ConvertToPrimaryConstructor
    public ArticulationGroup(
        ArticulationGroupName name,
        IReadOnlyCollection<Articulation> articulations,
        IReadOnlyDictionary<string, string>? extra = null )
    {
        Name          = name;
        Articulations = articulations;
        Extra         = extra != null ? new Dictionary<string, string>( extra ) : new Dictionary<string, string>();
    }

    public static ArticulationGroup Create(
        string name,
        IReadOnlyCollection<Articulation>? articulations = null,
        IReadOnlyDictionary<string, string>? extra = null )
    {
        return new ArticulationGroup(
            new ArticulationGroupName( name ),
            articulations != null ? new List<Articulation>( articulations ) : new List<Articulation>(),
            extra
        );
    }
}
