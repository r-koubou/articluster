using System;
using System.Collections.Generic;

using ArtiCluster.Shared.Domain.Articulation.Model.Values;

namespace ArtiCluster.Shared.Domain.Articulation.Model;

public sealed record Articulation(
    Guid Id,
    Author Author,
    ManufacturerName ManufacturerName,
    ProductName ProductName,
    PatchName PatchName,
    IReadOnlyCollection<Assignment> ArticulationMaps
);
