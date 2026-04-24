using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinitions.Gateways;
using ArtiCluster.Features.UniversalDefinitions.Infrastructures.Yaml.Model;
using ArtiCluster.Shared.Domain.Articulation.Model;
using ArtiCluster.Shared.IO.Abstractions;

using YamlDotNet.Core;

namespace ArtiCluster.Features.UniversalDefinitions.Infrastructures.Yaml;

public sealed class YamlImporter : IDefinitionImporter
{
    public async Task<Result<Articulation, ImportReason>> ImportAsync(
        ITextContentReader reader,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var yamlText = await reader.ReadAllAsync( cancellationToken );
            var deserializer = SerializationConstants.DefaultDeserializer;

            var root = deserializer.Deserialize<RootModel>( yamlText );
            var domain = DomainModelMapper.Map( root );

            return Result<Articulation, ImportReason>.Success( domain );
        }
        catch( YamlException e )
        {
            return Result<Articulation, ImportReason>.Failure( ImportReason.DeserializationError, e );
        }
        catch( IOException e )
        {
            return Result<Articulation, ImportReason>.Failure( ImportReason.IoError, e );
        }
        catch( Exception e )
        {
            return Result<Articulation, ImportReason>.Failure( ImportReason.OtherError, e );
        }
    }
}
