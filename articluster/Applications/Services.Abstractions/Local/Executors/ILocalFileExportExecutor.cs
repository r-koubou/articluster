namespace ArtiCluster.Applications.Services.Abstractions.Local.Executors;

public interface ILocalFileExportExecutor<TSource>
    : ILocalOutputExecutor<TSource, ExportFailureReason>;
