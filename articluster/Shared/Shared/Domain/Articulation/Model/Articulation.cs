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
        string author,
        string manufacturerName,
        string productName,
        string patchName,
        string? description,
        IReadOnlyCollection<Assignment> assignments,
        IReadOnlyDictionary<string, string>? extra = null )
    {
        Id               = id;
        Author           = new Author( author );
        ManufacturerName = new ManufacturerName( manufacturerName );
        ProductName      = new ProductName( productName );
        PatchName        = new PatchName( patchName );
        Description      = description == null ? Description.Empty : new Description( description );
        Assignments      = assignments;
        Extra            = extra == null ? new Dictionary<string, string>() : new Dictionary<string, string>( extra );
    }
}
