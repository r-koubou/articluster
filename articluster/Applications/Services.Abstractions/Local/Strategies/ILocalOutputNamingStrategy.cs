namespace ArtiCluster.Applications.Services.Abstractions.Local.Strategies;

public interface ILocalOutputNamingStrategy<in TSource>
{
    string GetOutputDirectory( string baseDirectory, TSource source );
    string GetOutputFileName( TSource source );
}
