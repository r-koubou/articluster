namespace ArtiCluster.Shared;

public interface IDataMapper<in TSource, out TTarget>
{
    TTarget Map( TSource source );
}
