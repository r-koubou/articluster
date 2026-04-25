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
        ManufacturerName = manufacturerName;
        Items = items
               .Where( x => x.ManufacturerName == ManufacturerName )
               .ToList();
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
