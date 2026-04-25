using ArtiCluster.Commons;
using ArtiCluster.Features.StudioOne.Gateways;
using ArtiCluster.Features.StudioOne.Infrastructures.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions;

namespace ArtiCluster.Features.StudioOne.Infrastructures;

public interface IStudioOneModelMapper
{
    Result<StudioOneRootElement, ExportReason> Map( UniversalDefinitionProductSet source );
}
