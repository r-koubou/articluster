namespace ArtiCluster.Applications.Services.Abstractions.Strategies;

public interface IExportNamingStrategy<in TSource>
{
    string GetOutputDirectory( string baseDirectory, TSource source );
    string GetOutputFileName( TSource source );
}
