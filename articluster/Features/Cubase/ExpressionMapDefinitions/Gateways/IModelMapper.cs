using ArtiCluster.Commons;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;

namespace ArtiCluster.Features.Cubase.ExpressionMapDefinitions.Gateways;

public interface IModelMapper<TModel>
{
    Result<TModel, ExportReason> Map( UniversalDefinition source );
}
