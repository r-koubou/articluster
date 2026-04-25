using System.Collections.Generic;
using System.Linq;

using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Shared.Domain.UniversalDefinitions;

public sealed record UniversalDefinitionProductCollection
{
    // ReSharper disable MemberCanBePrivate.Global
    public IReadOnlyCollection<UniversalDefinitionProductSet> Items { get; }

    public int Count
        => Items.Count;

    public bool IsEmpty
        => Items.Count == 0;
    // ReSharper restore MemberCanBePrivate.Global

    public UniversalDefinitionProductCollection(
        IEnumerable<UniversalDefinition> items )
    {
        Items = items
               .GroupBy( x => new
                    {
                        x.ManufacturerName,
                        x.ProductName
                    }
                )
               .Select( g => new UniversalDefinitionProductSet( g.Key.ManufacturerName, g.Key.ProductName, g.ToList() ) )
               .ToList();
    }

    public static UniversalDefinitionProductCollection Create(
        IEnumerable<UniversalDefinition> items )
    {
        return new UniversalDefinitionProductCollection( items );
    }
}
