using System.Collections.Generic;
using System.Linq;

using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model.Values;

namespace ArtiCluster.Shared.Domain.UniversalDefinitions;

public sealed record UniversalDefinitionProductSet
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

    public UniversalDefinitionProductSet(
        ManufacturerName manufacturerName,
        ProductName productName,
        IEnumerable<UniversalDefinition> items )
    {
        ManufacturerName = manufacturerName;
        ProductName      = productName;
        Items = items
               .Where( x => x.ManufacturerName == ManufacturerName && x.ProductName == ProductName )
               .ToList();
    }

    public static UniversalDefinitionProductSet Create(
        string manufacturerName,
        string productName,
        IEnumerable<UniversalDefinition> items )
    {
        return new UniversalDefinitionProductSet(
            new ManufacturerName( manufacturerName ),
            new ProductName( productName ),
            items
        );
    }
}
