using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.UniversalDefinitions.Contracts;
using ArtiCluster.Features.UniversalDefinitions.Mappers;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

using YamlDotNet.Core;

namespace ArtiCluster.Features.UniversalDefinitions.Exports;

public sealed class YamlExporter
{
    public async Task<Result<Unit, ExportFailureReason>> ExportAsync(
        ITextContentWriter writer,
        UniversalDefinition source,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var serializer = SerializationConstants.DefaultSerializer;

            var root = YamlModelMapper.Map( source );
            var yamlText = serializer.Serialize( root );

            await writer.WriteAsync( yamlText, cancellationToken );

            return Result<Unit, ExportFailureReason>.Success( Unit.Default );
        }
        catch( YamlException e )
        {
            return Result<Unit, ExportFailureReason>.Failure( ExportFailureReason.SerializationError, e );
        }
        catch( IOException e )
        {
            return Result<Unit, ExportFailureReason>.Failure( ExportFailureReason.IoError, e );
        }
        catch( Exception e )
        {
            return Result<Unit, ExportFailureReason>.Failure( ExportFailureReason.OtherError, e );
        }
    }
}
