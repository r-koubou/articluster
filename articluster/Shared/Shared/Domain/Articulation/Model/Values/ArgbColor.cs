using ArtiCluster.Commons.ValueObjects;

namespace ArtiCluster.Shared.Domain.Articulation.Model.Values;

public sealed record ArgbColor( int Value ) : IntValueObject( Value )
{
    public static readonly ArgbColor Black = new ArgbColor( 0xFF, 0, 0, 0 );
    public static readonly ArgbColor White = new ArgbColor( 0xFF, 0xFF, 0xFF, 0xFF );
    public static readonly ArgbColor Red = new ArgbColor( 0xFF, 0x00, 0xFF, 0x00 );
    public static readonly ArgbColor Green = new ArgbColor( 0x00, 0xFF, 0x00, 0xFF );
    public static readonly ArgbColor Blue = new ArgbColor( 0x00, 0x00, 0x00, 0xFF );
    public static readonly ArgbColor Yellow = new ArgbColor( 0xFF, 0xFF, 0xFF, 0xFF );
    public static readonly ArgbColor Cyan = new ArgbColor( 0x00, 0xFF, 0x00, 0xFF );
    public static readonly ArgbColor Magenta = new ArgbColor( 0x00, 0xFF, 0x00, 0xFF );

    public int A
        => ( Value >> 24 ) & 0xFF;

    public int R
        => Value & 0xFF;

    public int G
        => ( Value >> 8 ) & 0xFF;

    public int B
        => ( Value >> 16 ) & 0xFF;

    public ArgbColor( int a, int r, int g, int b )
        : this(
            ( ( a & 0xFF ) << 24 ) |
            ( ( b & 0xFF ) << 16 ) |
            ( ( g & 0xFF ) << 8 ) |
            ( r & 0xFF )
        ) {}

    public ArgbColor( int r, int g, int b )
        : this(
            ( ( b & 0xFF ) << 16 ) |
            ( ( g & 0xFF ) << 8 ) |
            ( r & 0xFF )
        ) {}
}
