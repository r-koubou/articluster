using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using ArtiCluster.Commons;
using ArtiCluster.Commons.IO;
using ArtiCluster.Features.Cubase.ExpressionMapDefinitions.Gateways;
using ArtiCluster.Features.Cubase.ExpressionMapDefinitions.Infrastructures.Model;
using ArtiCluster.Shared.Domain.UniversalDefinitions.Model;
using ArtiCluster.Shared.IO.Abstractions;

namespace ArtiCluster.Features.Cubase.ExpressionMapDefinitions.Infrastructures;

public sealed class CubaseExporter : IDefinitionExporter
{
    public async Task<Result<Unit, ExportReason>> ExportAsync( ITextContentWriter writer, UniversalDefinition source, CancellationToken cancellationToken = default )
    {
        try
        {
            var mapResult = new CubaseModelMapper().Map( source );

            if( mapResult.IsFailure )
            {
                return Result<Unit, ExportReason>.Failure( ExportReason.SerializationError );
            }

            var serializer = new XmlSerializer( typeof( RootElement ) );
            // no xmlns adding
            // see: https://stackoverflow.com/a/8882612
            var xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add( "", "" );

            var stringWriter = new StringWriterWithEncoding( Encoding.UTF8 );
            var xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                Async  = true
            };

            using var xmlWriter = XmlWriter.Create( stringWriter, xmlWriterSettings );
            serializer.Serialize( xmlWriter, mapResult.Unwrap(), xmlNamespaces );

            await writer.WriteAsync( stringWriter.ToString(), cancellationToken );

            return Result<Unit, ExportReason>.Success( Unit.Default );
        }
        catch( InvalidOperationException e )
        {
            return Result<Unit, ExportReason>.Failure( ExportReason.SerializationError, e );
        }
        catch( IOException e )
        {
            return Result<Unit, ExportReason>.Failure( ExportReason.IoError, e );
        }
    }
}
