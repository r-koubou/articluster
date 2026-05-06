using ArtiCluster.Commons.ValueObjects;

namespace ArtiCluster.Shared.Domain.UniversalDefinitions.Model.Values;

public sealed record PatchName( string Value ) : StringValueObject( Value )
{
    public override bool AllowEmpty
        => false;
}
