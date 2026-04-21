namespace ArtiCluster.Commons.ValueObjects;

public abstract record ValueObject<TValue>( TValue Value ) where TValue : notnull
{
    public sealed override string ToString()
        => Value.ToString();
}
