using ArtiCluster.Shared.ValueObjects;

namespace ArtiCluster.Shared.Domain.Articulation.Model.Values;

public sealed record ManufacturerName( string Value ) : StringValueObject( Value )
{
    public override bool AllowEmpty
        => false;
}
