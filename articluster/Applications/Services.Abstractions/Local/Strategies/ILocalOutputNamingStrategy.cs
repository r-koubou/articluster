namespace ArtiCluster.Applications.Services.Abstractions.Strategies;

public interface ILocalOutputNamingStrategy<in TSource>
{
    string GetOutputDirectory( string baseDirectory, TSource source );
    string GetOutputFileName( TSource source );
}
