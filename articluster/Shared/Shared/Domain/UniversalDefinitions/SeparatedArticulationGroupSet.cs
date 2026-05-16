using System;
using System.Collections.Generic;
using System.Linq;

using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model.Values;

namespace ArtiCluster.Shared.Domain.UniversalDefinitions;

/// <summary>
/// Represents a collection of <see cref="Articulation"/> classified by <see cref="ManufacturerName"/>, <see cref="ProductName"/>, <see cref="PatchName"/>, and <see cref="ArticulationGroup"/>.
/// </summary>
public sealed record SeparatedArticulationGroupSet
{
    // ReSharper disable MemberCanBePrivate.Global
    public ManufacturerName ManufacturerName { get; }

    public ProductName ProductName { get; }

    public PatchName PatchName { get; }

    public ArticulationGroupName ArticulationGroupName { get; }

    public IReadOnlyCollection<Articulation> Items { get; }

    public int Count
        => Items.Count;

    public bool IsEmpty
        => Items.Count == 0;
    // ReSharper restore MemberCanBePrivate.Global

    public SeparatedArticulationGroupSet(
        ManufacturerName manufacturerName,
        ProductName productName,
        PatchName patchName,
        ArticulationGroupName articulationGroupName,
        IEnumerable<Articulation> items )
    {
        if( items == null! )
        {
            throw new ArgumentNullException( nameof( items ) );
        }

        ManufacturerName      = manufacturerName;
        ProductName           = productName;
        ArticulationGroupName = articulationGroupName;
        PatchName             = patchName;
        Items                 = items.ToList();
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
