using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions;

namespace ArtiCluster.Features.Cakewalk.ArticulationMapDefinitions.Gateways;

public interface IModelMapper<TModel>
{
    Result<TModel, ExportReason> Map( UniversalDefinitionProductSet source );
}
