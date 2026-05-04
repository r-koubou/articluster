using ArtiCluster.Applications.Cli.Editor.Model;
using ArtiCluster.Commons.EventEmitting;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Applications.Cli.Editor;

public class ApplicationContext
{
    public IEventEmitter EventEmitter { get; }
    public UniversalDefinitionModel Current { get; set; }

    // ReSharper disable once ConvertToPrimaryConstructor
    public ApplicationContext( IEventEmitter eventEmitter, UniversalDefinitionModel? initialModel = null )
    {
        EventEmitter = eventEmitter;
        Current      = initialModel ?? new UniversalDefinitionModel();
    }
}
