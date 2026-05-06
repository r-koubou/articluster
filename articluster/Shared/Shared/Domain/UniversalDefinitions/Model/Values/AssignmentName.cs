using ArtiCluster.Commons.ValueObjects;

namespace ArtiCluster.Shared.Domain.UniversalDefinitions.Model.Values;

public sealed record AssignmentName( string Value ) : StringValueObject( Value )
{
    public override bool AllowEmpty
        => false;
}
