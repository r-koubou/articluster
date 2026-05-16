using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ArtiCluster.Commons;
using ArtiCluster.Features.Logic.Articulations.Contracts;
using ArtiCluster.Features.Logic.Articulations.Mappers;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

using Claunia.PropertyList;

namespace ArtiCluster.Features.Logic.Articulations.Exports;

public sealed class LogicExporter
{
    public async Task<Result<Unit, ExportFailureReason>> ExportAsync( ITextContentWriter writer, UniversalDefinition source, CancellationToken cancellationToken = default )
    {
        try
        {
            var mapper = new LogicModelMapper();
            var root = mapper.Map( source );
            using var memoryStream = new MemoryStream();

            PropertyListParser.SaveAsXml( root, memoryStream );
            memoryStream.Position = 0;

            using var textReader = new StreamReader( memoryStream );
            await writer.WriteAsync( await textReader.ReadToEndAsync( cancellationToken ), cancellationToken );

            return Result<Unit, ExportFailureReason>.Success( Unit.Default );
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
