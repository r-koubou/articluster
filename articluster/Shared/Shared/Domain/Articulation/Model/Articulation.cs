using System;
using System.Collections.Generic;

using ArtiCluster.Shared.Domain.Articulation.Model.Values;

namespace ArtiCluster.Shared.Domain.Articulation.Model;

public sealed record Articulation
{
    public Guid Id { get; init; }
    public Author Author { get; init; }
    public ManufacturerName ManufacturerName { get; init; }
    public ProductName ProductName { get; init; }
    public PatchName PatchName { get; init; }
    public Description Description { get; init; }
    public IReadOnlyCollection<Assignment> Assignments { get; init; }
    public IReadOnlyDictionary<string, string> Extra { get; init; }

    public Articulation(
        Guid id,
        Author author,
        ManufacturerName manufacturerName,
        ProductName productName,
        PatchName patchName,
        Description? description,
        IReadOnlyCollection<Assignment> assignments,
        IReadOnlyDictionary<string, string>? extra = null )
    {
        Id               = id;
        Author           = author;
        ManufacturerName = manufacturerName;
        ProductName      = productName;
        PatchName        = patchName;
        Assignments      = assignments;
        Description      = description ?? Description.Empty;
        Extra            = extra ?? new Dictionary<string, string>();
    }
}
