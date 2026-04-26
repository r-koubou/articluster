using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions;

namespace ArtiCluster.Features.StudioOne.KeySwitchDefinitions.Gateways;

public interface IModelMapper<TModel>
{
    Result<TModel, ExportReason> Map( UniversalDefinitionProductSet source );
}
