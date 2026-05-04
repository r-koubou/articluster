using ArtiCluster.Applications.Cli.Editor.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Applications.Cli.Editor;

public class ApplicationContext
{
    public UniversalDefinitionModel Current { get; set; } = UniversalDefinitionMapper.FromDomain( UniversalDefinition.CreateTemplate() );
}
