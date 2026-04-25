using System;
using System.Collections.Generic;
using System.Linq;

using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model.Values;

namespace ArtiCluster.Shared.Domain.UniversalDefinitions;

public sealed record UniversalDefinitionManufacturerSet
{
    // ReSharper disable MemberCanBePrivate.Global
    public ManufacturerName ManufacturerName { get; }

    public IReadOnlyCollection<UniversalDefinition> Items { get; }

    public int Count
        => Items.Count;

    public bool IsEmpty
        => Items.Count == 0;
    // ReSharper restore MemberCanBePrivate.Global

    public UniversalDefinitionManufacturerSet(
        ManufacturerName manufacturerName,
        IEnumerable<UniversalDefinition> items )
    {
        if( items == null! )
        {
            throw new ArgumentNullException( nameof( items ) );
        }

        var itemList = items.ToList();

        if( itemList.Any( x => x.ManufacturerName != manufacturerName ) )
        {
            throw new ArgumentException( "All items must match the specified manufacturerName.", nameof( items ) );
        }

        ManufacturerName = manufacturerName;
        Items            = itemList;

    }

    public static UniversalDefinitionManufacturerSet Create(
        string manufacturerName,
        IEnumerable<UniversalDefinition> items )
    {
        return new UniversalDefinitionManufacturerSet(
            new ManufacturerName( manufacturerName ),
            items
        );
    }
}
