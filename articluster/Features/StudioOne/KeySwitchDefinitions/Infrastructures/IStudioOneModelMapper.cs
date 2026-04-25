using ArtiCluster.Commons;
using ArtiCluster.Features.StudioOne.KeySwitchDefinitions.Gateways;
using ArtiCluster.Features.StudioOne.KeySwitchDefinitions.Infrastructures.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions;

namespace ArtiCluster.Features.StudioOne.KeySwitchDefinitions.Infrastructures;

public interface IStudioOneModelMapper
{
    Result<StudioOneRootElement, ExportReason> Map( UniversalDefinitionProductSet source );
}
