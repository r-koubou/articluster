namespace ArtiCluster.Commons;

public interface IDataMapper<in TSource, out TTarget>
{
    TTarget Map( TSource source );
}
