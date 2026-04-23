using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinition.Gateways;
using ArtiCluster.Shared.Domain.Articulation.Model;
using ArtiCluster.Shared.IO.Abstractions;

using YamlDotNet.Core;

namespace ArtiCluster.Features.UniversalDefinition.Infrastructures.Yaml;

public sealed class YamlExporter : IDefinitionExporter
{
    public async Task<Result<Unit, ExportReason>> ExportAsync(
        ITextContentWriter writer,
        Articulation source,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var serializer = SerializationConstants.DefaultSerializer;

            var root = YamlModelMapper.Map( source );
            var yamlText = serializer.Serialize( root );

            await writer.WriteAsync( yamlText, cancellationToken );

            return Result<Unit, ExportReason>.Success( Unit.Default );
        }
        catch( YamlException e )
        {
            return Result<Unit, ExportReason>.Failure( ExportReason.SerializationError, e );
        }
        catch( IOException e )
        {
            return Result<Unit, ExportReason>.Failure( ExportReason.IoError, e );
        }
        catch( Exception e )
        {
            return Result<Unit, ExportReason>.Failure( ExportReason.OtherError, e );
        }
    }
}
