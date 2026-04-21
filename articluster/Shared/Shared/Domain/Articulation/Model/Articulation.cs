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
    public IReadOnlyCollection<Assignment> ArticulationMaps { get; init; }
    public IReadOnlyDictionary<string, string> Extra { get; init; }

    public Articulation(
        Guid Id,
        Author Author,
        ManufacturerName ManufacturerName,
        ProductName ProductName,
        PatchName PatchName,
        Description? Description,
        IReadOnlyCollection<Assignment> ArticulationMaps,
        IReadOnlyDictionary<string, string>? Extra = null )
    {
        this.Id               = Id;
        this.Author           = Author;
        this.ManufacturerName = ManufacturerName;
        this.ProductName      = ProductName;
        this.PatchName        = PatchName;
        this.ArticulationMaps = ArticulationMaps;
        this.Description      = Description ?? Description.Empty;
        this.Extra            = Extra ?? new Dictionary<string, string>();
    }
}
