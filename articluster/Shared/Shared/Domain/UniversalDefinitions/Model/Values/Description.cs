using ArtiCluster.Commons.ValueObjects;

namespace ArtiCluster.Shared.Domain.UniversalDefinitions.Model.Values;

public sealed record Description( string Value ) : StringValueObject( Value )
{
    public override bool AllowEmpty
        => true;

    public static Description Empty
        => new( string.Empty );
}
