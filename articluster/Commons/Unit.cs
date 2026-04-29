using System;

namespace ArtiCluster.Commons;

[Serializable]
public sealed class Unit : IEquatable<Unit>
{
    public static readonly Unit Default = new();

    private Unit() {}

    public bool Equals( Unit? other )
        => other == Default;

    public override bool Equals( object? obj )
        => ReferenceEquals( this, obj ) || obj is Unit other && Equals( other );

    public override int GetHashCode()
        => 1;

    public static bool operator ==( Unit? left, Unit? right )
        => ReferenceEquals( left, right ) || left != null && right != null && left.Equals( right );

    public static bool operator !=( Unit? left, Unit? right )
        => !( left == right );
}
