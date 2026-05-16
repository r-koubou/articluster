using System;
using System.Collections.Generic;
using System.Linq;

using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Shared.Domain.UniversalDefinitions;

public sealed record ProductCollection
{
    // ReSharper disable MemberCanBePrivate.Global
    public IReadOnlyCollection<UniversalDefinitionProductSet> Items { get; }

    public int Count
        => Items.Count;

    public bool IsEmpty
        => Items.Count == 0;
    // ReSharper restore MemberCanBePrivate.Global

    public ProductCollection(
        IEnumerable<UniversalDefinition> items )
    {
        if( items == null! )
        {
            throw new ArgumentNullException( nameof( items ) );
        }

        Items = items
               .GroupBy( x => new
                    {
                        x.ManufacturerName,
                        x.ProductName
                    }
                )
               .Select( g => new UniversalDefinitionProductSet( g.Key.ManufacturerName, g.Key.ProductName, g ) )
               .ToList();
    }
}
