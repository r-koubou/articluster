using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinition.Gateways;
using ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml.Model;
using ArtiCluster.Shared.Domain.Articulation.Model;
using ArtiCluster.Shared.IO.Abstractions;

using YamlDotNet.Core;

namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml;

internal sealed class YamlImporter : IDefinitionImporter
{
    public async Task<Result<Articulation, ImporterReason>> ImportAsync(
        ITextContentReader reader,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var yamlText = await reader.ReadAllAsync( cancellationToken );
            var deserializer = SerializationConstants.DefaultDeserializer;

            var root = deserializer.Deserialize<RootModel>( yamlText );
            var domain = DomainModelMapper.Map( root );

            return Result<Articulation, ImporterReason>.Success( domain, ImporterReason.Ok );
        }
        catch( YamlException e )
        {
            return Result<Articulation, ImporterReason>.Failure( ImporterReason.DeserializationError, e );
        }
        catch( IOException e )
        {
            return Result<Articulation, ImporterReason>.Failure( ImporterReason.IoError, e );
        }
        catch( Exception e )
        {
            return Result<Articulation, ImporterReason>.Failure( ImporterReason.OtherError, e );
        }
    }
}
