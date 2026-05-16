using System;
using System.Collections.Generic;
using System.Linq;

using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Shared.Domain.UniversalDefinitions;

/// <summary>
/// A Collection of <see cref="SeparatedArticulationGroupSet"/>
/// </summary>
public sealed record SeparatedArticulationGroupCollection
{
    // ReSharper disable MemberCanBePrivate.Global
    public IReadOnlyCollection<SeparatedArticulationGroupSet> Items { get; }

    public int Count
        => Items.Count;

    public bool IsEmpty
        => Items.Count == 0;
    // ReSharper restore MemberCanBePrivate.Global

    public SeparatedArticulationGroupCollection(
        IEnumerable<UniversalDefinition> items )
    {
        ArgumentNullException.ThrowIfNull( items );

        // @formatter:off
        var itemList = items
           .GroupBy( x => new
                {
                    x.ManufacturerName,
                    x.ProductName
                }
            )
           .SelectMany( g =>
                g.SelectMany( x =>
                    x.ArticulationGroups.Select( ag =>
                        new SeparatedArticulationGroupSet(
                            g.Key.ManufacturerName,
                            g.Key.ProductName,
                            x.PatchName,
                            ag.Name,
                            ag.Articulations
                        )
                    )
                )
            )
           .ToList();
        // @formatter:on

        Items = itemList;
    }

    public static ProductCollection Create(
        IEnumerable<UniversalDefinition> items )
    {
        return new ProductCollection( items );
    }

    public IEnumerable<SeparatedArticulationGroupSet> EnumerateDefinitions()
    {
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach( var x in Items )
        {
            yield return x;
        }
    }
}
