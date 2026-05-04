using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using ArtiCluster.Commons;
using ArtiCluster.Commons.IO;
using ArtiCluster.Commons.Text;
using ArtiCluster.Features.Cubase.ExpressionMaps.Contracts;
using ArtiCluster.Features.Cubase.ExpressionMaps.Mappers;
using ArtiCluster.Features.Cubase.ExpressionMaps.Models;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.Cubase.ExpressionMaps.Exports;

public sealed class CubaseExporter
{
    public async Task<Result<Unit, ExportFailureReason>> ExportAsync( ITextContentWriter writer, UniversalDefinition source, CancellationToken cancellationToken = default )
    {
        try
        {
            var mapResult = new CubaseModelMapper().Map( source );

            if( mapResult.IsFailure )
            {
                return Result<Unit, ExportFailureReason>.Failure( ExportFailureReason.SerializationError );
            }

            var serializer = new XmlSerializer( typeof( RootElement ) );
            // no xmlns adding
            // see: https://stackoverflow.com/a/8882612
            var xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add( "", "" );

            var stringWriter = new StringWriterWithEncoding( EncodingConstants.Utf8NoBom );
            var xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                Async  = true
            };

            using var xmlWriter = XmlWriter.Create( stringWriter, xmlWriterSettings );
            serializer.Serialize( xmlWriter, mapResult.Unwrap(), xmlNamespaces );

            await writer.WriteAsync( stringWriter.ToString(), cancellationToken );

            return Result<Unit, ExportFailureReason>.Success( Unit.Default );
        }
        catch( InvalidOperationException e )
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
