namespace ArtiCluster.Commons.ValueObjects;

public abstract record StringValueObject : ValueObject<string>
{
    public abstract bool AllowEmpty { get; }

    protected StringValueObject( string Value ) : base( Value )
    {
        EmptyStringValueException.ThrowIfEmpty( this );
    }
}
