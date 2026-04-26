using System;
using System.Collections.Generic;

using ArtiCluster.Shared.Domain.UniversalDefinitions.Model.Values;

namespace ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

public sealed record UniversalDefinition
{
    public Guid Id { get; init; }
    public Author Author { get; init; }
    public ManufacturerName ManufacturerName { get; init; }
    public ProductName ProductName { get; init; }
    public PatchName PatchName { get; init; }
    public Description Description { get; init; }

    public IReadOnlyCollection<Articulation> Articulations { get; init; }
    public IReadOnlyDictionary<string, string> Extra { get; init; }

    public UniversalDefinition(
        Guid id,
        Author author,
        ManufacturerName manufacturerName,
        ProductName productName,
        PatchName patchName,
        Description? description,
        IReadOnlyCollection<Articulation> articulations,
        IReadOnlyDictionary<string, string>? extra = null )
    {
        Id               = id;
        Author           = author;
        ManufacturerName = manufacturerName;
        ProductName      = productName;
        PatchName        = patchName;
        Description      = description ?? Description.Empty;
        Articulations    = articulations;
        Extra            = extra == null ? new Dictionary<string, string>() : new Dictionary<string, string>( extra );
    }

    public static UniversalDefinition Create(
        Guid id,
        string author,
        string manufacturerName,
        string productName,
        string patchName,
        string? description,
        IReadOnlyCollection<Articulation> articulations,
        IReadOnlyDictionary<string, string>? extra = null )
    {
        return new UniversalDefinition(
            id,
            new Author( author ),
            new ManufacturerName( manufacturerName ),
            new ProductName( productName ),
            new PatchName( patchName ),
            description != null ? new Description( description ) : null,
            articulations,
            extra
        );
    }
}
