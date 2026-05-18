using System;
using System.Collections.Generic;
using System.Linq;

using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model.Values;

namespace ArtiCluster.Shared.Domain.UniversalDefinitions;

public sealed record ProductSet
{
    // ReSharper disable MemberCanBePrivate.Global
    public ManufacturerName ManufacturerName { get; }

    public ProductName ProductName { get; }

    public IReadOnlyCollection<UniversalDefinition> Items { get; }

    public int Count
        => Items.Count;

    public bool IsEmpty
        => Items.Count == 0;
    // ReSharper restore MemberCanBePrivate.Global

    public ProductSet(
        ManufacturerName manufacturerName,
        ProductName productName,
        IEnumerable<UniversalDefinition> items )
    {
        if( items == null! )
        {
            throw new ArgumentNullException( nameof( items ) );
        }

        var itemList = items.ToList();

        if( itemList.Any( x => x.ManufacturerName != manufacturerName || x.ProductName != productName ) )
        {
            throw new ArgumentException( "All items must match the specified manufacturerName and productName.", nameof( items ) );
        }

        ManufacturerName = manufacturerName;
        ProductName      = productName;
        Items            = itemList;
    }

    public static ProductSet Create(
        string manufacturerName,
        string productName,
        IEnumerable<UniversalDefinition> items )
    {
        return new ProductSet(
            new ManufacturerName( manufacturerName ),
            new ProductName( productName ),
            items
        );
    }
}
