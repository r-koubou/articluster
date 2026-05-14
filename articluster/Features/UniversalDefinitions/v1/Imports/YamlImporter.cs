using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinitions.Contracts;
using ArtiCluster.Features.UniversalDefinitions.v1.Mappers;
using ArtiCluster.Features.UniversalDefinitions.v1.Models;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

using YamlDotNet.Core;

namespace ArtiCluster.Features.UniversalDefinitions.v1.Imports;

public sealed class YamlImporter
{
    public async Task<Result<UniversalDefinition, ImportFailureReason>> ImportAsync(
        ITextContentReader reader,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var yamlText = await reader.ReadAllAsync( cancellationToken );
            var deserializer = DeserializationConstants.DefaultDeserializer;

            var root = deserializer.Deserialize<UniversalDefinitionModel>( yamlText );
            var domain = DomainModelMapper.Map( root );

            return Result<UniversalDefinition, ImportFailureReason>.Success( domain );
        }
        catch( YamlException e )
        {
            return Result<UniversalDefinition, ImportFailureReason>.Failure( ImportFailureReason.DeserializationError, e );
        }
        catch( UnsupportedFormatVersionException e )
        {
            return Result<UniversalDefinition, ImportFailureReason>.Failure( ImportFailureReason.UnsupportedFormatVersion, e );
        }
        catch( IOException e )
        {
            return Result<UniversalDefinition, ImportFailureReason>.Failure( ImportFailureReason.IoError, e );
        }
        catch( Exception e )
        {
            return Result<UniversalDefinition, ImportFailureReason>.Failure( ImportFailureReason.OtherError, e );
        }
    }
}
